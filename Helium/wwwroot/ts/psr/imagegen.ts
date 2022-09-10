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
    pendingJobs: ko.Observable<string>

    jobs: ko.ObservableArray<string>
    selectedJob: ko.Observable<string>

    image: ko.Observable<string>

    jobLoader: ko.Computed

    constructor() {
        this.accessKey = ko.observable("");
        this.prompt = ko.observable("");

        this.jobStatus = ko.observable("Login to continue");
        this.backendStatus = ko.observable("Idle");
        this.pendingJobs = ko.observable("? Pending Jobs");

        this.jobs = ko.observableArray(["No jobs"])
        this.selectedJob = ko.observable("");

        this.image = ko.observable("");

        this.jobLoader = ko.computed(() => {
            if (("" + this.selectedJob()).length < 8) {
                return;
            }

            // Load image from the server whenever the selected job changes
            this.jobStatus("Loading " + this.selectedJob().substring(0, this.selectedJob().length - 37));

            let jobIdComponents = this.selectedJob().split(":");
            let jobId = jobIdComponents[jobIdComponents.length - 1];
            let encodedAccessKey = encodeURIComponent(this.accessKey());
            let encodedJobId = encodeURIComponent(jobId);

            axios.get("/api/ImageGen/JobResults?accessKey=" + encodedAccessKey + "&jobId=" + encodedJobId)
                .then((response) => {
                    let data: JobResults = response.data
                    if (data.error != "") {
                        this.backendStatus(this.Truncate(data.error));
                    } else if (data.status != "") {
                        this.backendStatus(this.Truncate(data.status));
                    }
                    else {
                        this.jobStatus(this.selectedJob().substring(0, this.selectedJob().length - 37));
                        this.backendStatus("Loaded image");
                        this.image(data.imageData);

                        this.UpdatePendingJobs(encodedAccessKey);
                    }
                })
                .catch((err) => {
                    this.backendStatus(this.Truncate("Error: " + JSON.stringify(err)));
                });
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

        this.UpdatePendingJobs(encodedAccessKey)
    }

    UpdatePendingJobs(encodedAccessKey: string) {
        axios.get("/api/ImageGen/PendingJobsCount?accessKey=" + encodedAccessKey)
            .then((response) => {
                let data: PendingJobsResult = response.data
                if (data.error != "") {
                    this.backendStatus(this.Truncate(data.error));
                } else {
                    this.pendingJobs(data.count + " Pending Jobs");
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
                }
            })
            .catch((err) => {
                this.backendStatus(this.Truncate("Error: " + JSON.stringify(err)));
            });

        this.UpdatePendingJobs(encodedAccessKey);
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