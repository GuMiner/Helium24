import axios from "../../lib/axios-0.24.0.min"
import ko = require("../../lib/knockout-3.5.1.min")
import { UtilityModel } from "./utility"


interface JobQueueResult {
    userName: string,
    jobId: string,
    error: string,
}

interface JobsResult {
    error: string,
    userName: string,
    jobs: string[]
}

interface JobResults {
    error: string,
    status: string
    imageData: string
}

class ImageGenModel {
    accessKey: ko.Observable<string>
    prompt: ko.Observable<string>

    jobStatus: ko.Observable<string>
    backendStatus: ko.Observable<string>

    jobs: ko.ObservableArray<string>
    selectedJob: ko.Observable<string>

    image: ko.Observable<string>

    jobLoader: ko.Computed

    constructor() {
        this.accessKey = ko.observable("");
        this.prompt = ko.observable("");

        this.jobStatus = ko.observable("");
        this.backendStatus = ko.observable("Idle");

        this.jobs = ko.observableArray(["No jobs"])
        this.selectedJob = ko.observable("");

        this.jobLoader = ko.computed(() => {
            // Load image from the server whenever the selected job changes
            this.jobStatus("Loading " + this.selectedJob());

            let jobId = this.selectedJob().split(":")[0];
            let encodedAccessKey = encodeURIComponent(this.accessKey());
            let encodedJobId = encodeURIComponent(jobId);

            axios.get("/api/ImageGen/JobResults?accessKey=" + encodedAccessKey + "&jobId=" + encodedJobId)
                .then((response) => {
                    let data: JobResults = response.data
                    if (data.error != "") {
                        this.backendStatus(data.error);
                    } else if (data.status != "") {
                        this.backendStatus(data.status);
                    }
                    else {
                        this.backendStatus("Loaded image");
                        this.image(data.imageData);
                    }
                })
                .catch((err) => {
                    this.backendStatus("Error: " + JSON.stringify(err));
                });
        })
    }

    LoadJobs() {
        let encodedAccessKey = encodeURIComponent(this.accessKey());

        axios.get("/api/ImageGen/Jobs?accessKey=" + encodedAccessKey)
            .then((response) => {
                let data: JobsResult = response.data
                if (data.error != "") {
                    this.backendStatus(data.error);
                } else {
                    this.jobs(data.jobs);
                    this.backendStatus("Welcome " + data.userName);
                }
            })
            .catch((err) => {
                this.backendStatus("Error: " + JSON.stringify(err));
            });
    }

    QueueJob() {
        let encodedAccessKey = encodeURIComponent(this.accessKey());
        let encodedPrompt = encodeURIComponent(this.prompt());

        axios.post("/api/ImageGen/QueueJob?accessKey=" + encodedAccessKey + "&prompt=" + encodedPrompt)
            .then((response) => {
                let data: JobQueueResult = response.data
                if (data.error != "") {
                    this.backendStatus(data.error);
                } else {
                    this.jobStatus("Queued job " + data.jobId);
                    this.backendStatus("Welcome " + data.userName);

                    // Reload current jobs after the new job was queued
                    this.LoadJobs();
                }
            })
            .catch((err) => {
                this.backendStatus("Error: " + JSON.stringify(err));
            });
    }
}


class MainModel {
    utility: UtilityModel
    imageGen: ImageGenModel

    constructor() {
        this.utility = new UtilityModel();
        this.imageGen = new ImageGenModel();
    }
}

ko.applyBindings(new MainModel());