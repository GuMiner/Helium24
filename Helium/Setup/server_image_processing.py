import glob, os, sys, time
import requests
import pathlib
import cv2
import torch
import numpy as np
from omegaconf import OmegaConf
from PIL import Image
from tqdm import tqdm, trange
from itertools import islice
from einops import rearrange
from torchvision.utils import make_grid
from pytorch_lightning import seed_everything
from torch import autocast

from ldm.util import instantiate_from_config
from ldm.models.diffusion.ddim import DDIMSampler
from ldm.models.diffusion.plms import PLMSSampler

N_SAMPLES = 1
WIDTH = 512
HEIGHT = 512

LATENT_CHANNELS = 4
DOWNSAMPLING = 8

CONFIG = "configs/stable-diffusion/v1-inference.yaml"
CHECKPOINT = "models/ldm/stable-diffusion-v1/sd-v1-4.ckpt"

# TODO Configurable
SEED = 341076
SCALE = 7.5
ITERATIONS = 1
DDIM_ETA = 0.0
DDIM_STEPS = 50



def chunk(it, size):
    it = iter(it)
    return iter(lambda: tuple(islice(it, size)), ())


def load_model_from_config(config, verbose=False):
    print(f"Loading model from {CHECKPOINT}")
    pl_sd = torch.load(CHECKPOINT, map_location="cpu")
    if "global_step" in pl_sd:
        print(f"Global Step: {pl_sd['global_step']}")
    sd = pl_sd["state_dict"]
    model = instantiate_from_config(config.model)
    m, u = model.load_state_dict(sd, strict=False)
    if len(m) > 0 and verbose:
        print("missing keys:")
        print(m)
    if len(u) > 0 and verbose:
        print("unexpected keys:")
        print(u)

    model.cuda()
    model.eval()
    return model.half()


def main():
    image_gen_folder = "D:\imageGen"

    while True:
        response = requests.get("https://helium24.net/api/ImageGen/GetNextJob")
        if not response.ok:
            print('Could not talk to H24. Sleeping then continuing.')
            time.sleep(30)
            continue

        data = response.json()
        job_id = data["jobID"]
        prompt = data["prompt"]
        print(data)
        if job_id != "":
            # Cache prompt
            job_folder = os.path.join(image_gen_folder, job_id)
            if not os.path.exists(job_folder):
                os.mkdir(job_folder)
            pathlib.Path(os.path.join(job_folder, "prompt.txt")).write_text(prompt)
        
            generate_image(job_folder, prompt)
            
            # Upload
            with open(os.path.join(job_folder, "0.jpg"), 'rb') as file_content:
                headers = {'Content-Type': 'application/octet-stream'}
                response = requests.post(f"https://helium24.net/api/ImageGen/CompleteJob?jobId={job_id}", headers=headers, data=file_content)
                if not response.ok:
                    print(f'Could not upload {job_id}. May try again later')

            time.sleep(1)
        else:
            print(f"No jobs ready.")
            time.sleep(10)


def generate_image(job_folder, prompt):
    print(f"Generating image for job {job_folder}")

    # TODO speed this up by moving global defaults somewhere else
    seed_everything(SEED)
    img_count = 0

    config = OmegaConf.load(CONFIG)
    model = load_model_from_config(config)

    device = torch.device("cuda") if torch.cuda.is_available() else torch.device("cpu")
    model = model.to(device)

    # sampler = PLMSSampler(model)
    sampler = DDIMSampler(model)

    data = [N_SAMPLES * [prompt]]

    precision_scope = autocast
    with torch.no_grad():
        with precision_scope("cuda"):
            with model.ema_scope():
                all_samples = list()
                for n in trange(ITERATIONS, desc="Sampling"):
                    for prompts in tqdm(data, desc="data"):
                        uc = None
                        if SCALE != 1.0:
                            uc = model.get_learned_conditioning(N_SAMPLES * [""])
                        if isinstance(prompts, tuple):
                            prompts = list(prompts)
                        c = model.get_learned_conditioning(prompts)
                        shape = [LATENT_CHANNELS, HEIGHT // DOWNSAMPLING, WIDTH // DOWNSAMPLING]
                        samples_ddim, _ = sampler.sample(S=DDIM_STEPS,
                                                         conditioning=c,
                                                         batch_size=N_SAMPLES,
                                                         shape=shape,
                                                         verbose=False,
                                                         unconditional_guidance_scale=SCALE,
                                                         unconditional_conditioning=uc,
                                                         eta=DDIM_ETA,
                                                         x_T=None)

                        x_samples_ddim = model.decode_first_stage(samples_ddim)
                        x_samples_ddim = torch.clamp((x_samples_ddim + 1.0) / 2.0, min=0.0, max=1.0)
                        x_samples_ddim = x_samples_ddim.cpu().permute(0, 2, 3, 1).numpy()

                        x_checked_image_torch = torch.from_numpy(x_samples_ddim).permute(0, 3, 1, 2)

                        for x_sample in x_checked_image_torch: # Only one element as iterations is 1
                            x_sample = 255. * rearrange(x_sample.cpu().numpy(), 'c h w -> h w c')
                            img = Image.fromarray(x_sample.astype(np.uint8))
                            img.save(os.path.join(job_folder, f"{img_count}.jpg"))
                            img_count += 1


if __name__ == "__main__":
    main()
