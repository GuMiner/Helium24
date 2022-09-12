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

interface PendingJobsResult {
    error: string,
    count: number
}

class ImageGenModel {
    accessKey: ko.Observable<string>
    prompt: ko.Observable<string>

    jobStatus: ko.Observable<string>
    backendStatus: ko.Observable<string>

    countdown: number
    lastPendingJobs: string
    pendingJobs: ko.Observable<string>

    jobs: ko.ObservableArray<string>
    selectedJob: ko.Observable<string>

    encodedJobIdToLoad: string
    imageCountdown: number
    newImageTimer: number

    image: ko.Observable<string>

    jobLoader: ko.Computed

    constructor() {
        this.accessKey = ko.observable("");
        this.prompt = ko.observable("");

        this.jobStatus = ko.observable("Login to continue");
        this.backendStatus = ko.observable("Idle");

        // Periodically check for a new image if one is queued
        this.newImageTimer = -1;
        this.imageCountdown = 10;

        // Periodically update the pending jobs
        this.countdown = 10;
        this.lastPendingJobs = "?";
        this.pendingJobs = ko.observable("? Pending Jobs");
        window.setInterval(this.TimedUpdatePendingJobs, 1000, this);

        this.jobs = ko.observableArray(["No jobs"])
        this.selectedJob = ko.observable("");

        this.image = ko.observable("");

        this.jobLoader = ko.computed(() => {
            if (this.selectedJob() === undefined) {
                return;
            }

            // Load image from the server whenever the selected job changes
            this.jobStatus("Loading " + this.selectedJob().substring(0, this.selectedJob().length - 37));

            let jobIdComponents = this.selectedJob().split(":");
            let jobId = jobIdComponents[jobIdComponents.length - 1];
            let encodedAccessKey = encodeURIComponent(this.accessKey());
            let encodedJobId = encodeURIComponent(jobId);

            this.LoadImage(encodedAccessKey, encodedJobId);
        })
    }

    Truncate(input: string): string {
        if (input.length < 100) {
            return input;
        }
        else {
            return input.substring(0, 100);
        }
    }

    LoadJobs() {
        let encodedAccessKey = encodeURIComponent(this.accessKey());

        this.backendStatus("Loading jobs...");
        axios.get("/api/ImageGen/Jobs?accessKey=" + encodedAccessKey)
            .then((response) => {
                let data: JobsResult = response.data
                if (data.error != "") {
                    this.backendStatus(this.Truncate(data.error));
                } else {
                    this.jobs(data.jobs);
                    this.backendStatus("Welcome " + data.userName);
                    this.jobStatus("Select job or queue job")
                }
            })
            .catch((err) => {
                this.backendStatus(this.Truncate("Error: " + JSON.stringify(err)));
            });

        this.UpdatePendingJobs(encodedAccessKey);
    }

    TimedUpdatePendingJobs(self: ImageGenModel) {
        self.countdown = self.countdown - 1;

        if (self.countdown == 0) {
            self.countdown = 10;

            self.UpdatePendingJobs(encodeURIComponent(self.accessKey()));
        }

        self.pendingJobs(self.lastPendingJobs + " Pending Jobs (" + self.countdown + ")");
    }

    UpdatePendingJobs(encodedAccessKey: string) {
        axios.get("/api/ImageGen/PendingJobsCount?accessKey=" + encodedAccessKey)
            .then((response) => {
                let data: PendingJobsResult = response.data
                if (data.error != "") {
                    this.backendStatus(this.Truncate(data.error));
                } else {
                    this.lastPendingJobs = data.count + "";
                }
            })
            .catch((err) => {
                this.backendStatus(this.Truncate("Error: " + JSON.stringify(err)));
            });
    }

    QueueJob() {
        let encodedAccessKey = encodeURIComponent(this.accessKey());
        let encodedPrompt = encodeURIComponent(this.prompt());

        axios.post("/api/ImageGen/QueueJob?accessKey=" + encodedAccessKey + "&prompt=" + encodedPrompt)
            .then((response) => {
                let data: JobQueueResult = response.data
                if (data.error != "") {
                    this.backendStatus(this.Truncate(data.error));
                } else {
                    this.jobStatus("Queued job " + data.jobId);
                    this.backendStatus("Welcome " + data.userName);

                    // Reload current jobs after the new job was queued
                    this.LoadJobs();
                    this.encodedJobIdToLoad = encodeURIComponent(data.jobId);
                    this.newImageTimer = window.setInterval(this.TimedCheckForImage, 1000, this);
                }
            })
            .catch((err) => {
                this.backendStatus(this.Truncate("Error: " + JSON.stringify(err)));
            });

        this.UpdatePendingJobs(encodedAccessKey);
    }

    TimedCheckForImage(self: ImageGenModel) {
        self.imageCountdown = self.imageCountdown - 1;
        self.backendStatus("Image Generating (" + self.imageCountdown + ")")
        if (self.imageCountdown == 0) {
            let encodedAccessKey = encodeURIComponent(self.accessKey());
            self.LoadImage(encodedAccessKey, self.encodedJobIdToLoad);

            self.imageCountdown = 10;
        }
    }

    LoadImage(encodedAccessKey: string, encodedJobId: string) {
        axios.get("/api/ImageGen/JobResults?accessKey=" + encodedAccessKey + "&jobId=" + encodedJobId)
            .then((response) => {
                let data: JobResults = response.data
                if (data.error != "") {
                    this.backendStatus(this.Truncate(data.error));
                } else if (data.status != "") {
                    this.backendStatus(this.Truncate(data.status));
                }
                else {
                    this.backendStatus("Loaded image");
                    this.image(data.imageData);

                    this.UpdatePendingJobs(encodedAccessKey);

                    window.clearInterval(this.newImageTimer);

                    this.jobStatus(this.selectedJob().substring(0, this.selectedJob().length - 37));
                }
            })
            .catch((err) => {
                this.backendStatus(this.Truncate("Error: " + JSON.stringify(err)));
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