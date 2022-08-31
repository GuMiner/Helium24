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

class ImageGenModel {
    accessKey: ko.Observable<string>
    prompt: ko.Observable<string>

    jobStatus: ko.Observable<string>
    backendStatus: ko.Observable<string>

    jobs: ko.ObservableArray<string>
    selectedJob: ko.Observable<string>

    jobLoader: ko.Computed

    constructor() {
        this.accessKey = ko.observable("");
        this.prompt = ko.observable("");

        this.jobStatus = ko.observable("");
        this.backendStatus = ko.observable("Idle");

        this.jobs = ko.observableArray(["No jobs"])
        this.selectedJob = ko.observable("");

        this.jobLoader = ko.computed(() => {
            this.jobStatus("Loading " + this.selectedJob());

            // TODO: Get byte image data and assign it to the central image
            // TODO: Write backend to generate that image
            // TODO: Have website read image from backend if generation complete and save in DB.

            // let encodedQuery = encodeURIComponent(this.query());
            // axios.get("/api/CrosswordSearch/FindMatchingWords?search=" + encodedQuery)
            //     .then((response) => {
            //     })
            //     .catch((err) => {
            //         // this.dbStatus("Error: " + JSON.stringify(err));
            //     });
            // 
            // return encodedQuery;
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