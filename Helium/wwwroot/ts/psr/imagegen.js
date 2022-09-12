var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
define(["require", "exports", "../../lib/axios-0.24.0.min", "../../lib/knockout-3.5.1.min", "./utility"], function (require, exports, axios_0_24_0_min_1, ko, utility_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    axios_0_24_0_min_1 = __importDefault(axios_0_24_0_min_1);
    var ImageGenModel = /** @class */ (function () {
        function ImageGenModel() {
            var _this = this;
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
            this.jobs = ko.observableArray(["No jobs"]);
            this.selectedJob = ko.observable("");
            this.image = ko.observable("");
            this.jobLoader = ko.computed(function () {
                if (_this.selectedJob() === undefined) {
                    return;
                }
                // Load image from the server whenever the selected job changes
                _this.jobStatus("Loading " + _this.selectedJob().substring(0, _this.selectedJob().length - 37));
                var jobIdComponents = _this.selectedJob().split(":");
                var jobId = jobIdComponents[jobIdComponents.length - 1];
                var encodedAccessKey = encodeURIComponent(_this.accessKey());
                var encodedJobId = encodeURIComponent(jobId);
                _this.LoadImage(encodedAccessKey, encodedJobId);
            });
        }
        ImageGenModel.prototype.Truncate = function (input) {
            if (input.length < 100) {
                return input;
            }
            else {
                return input.substring(0, 100);
            }
        };
        ImageGenModel.prototype.LoadJobs = function () {
            var _this = this;
            var encodedAccessKey = encodeURIComponent(this.accessKey());
            this.backendStatus("Loading jobs...");
            axios_0_24_0_min_1.default.get("/api/ImageGen/Jobs?accessKey=" + encodedAccessKey)
                .then(function (response) {
                var data = response.data;
                if (data.error != "") {
                    _this.backendStatus(_this.Truncate(data.error));
                }
                else {
                    _this.jobs(data.jobs);
                    _this.backendStatus("Welcome " + data.userName);
                    _this.jobStatus("Select job or queue job");
                }
            })
                .catch(function (err) {
                _this.backendStatus(_this.Truncate("Error: " + JSON.stringify(err)));
            });
            this.UpdatePendingJobs(encodedAccessKey);
        };
        ImageGenModel.prototype.TimedUpdatePendingJobs = function (self) {
            self.countdown = self.countdown - 1;
            if (self.countdown == 0) {
                self.countdown = 10;
                self.UpdatePendingJobs(encodeURIComponent(self.accessKey()));
            }
            self.pendingJobs(self.lastPendingJobs + " Pending Jobs (" + self.countdown + ")");
        };
        ImageGenModel.prototype.UpdatePendingJobs = function (encodedAccessKey) {
            var _this = this;
            axios_0_24_0_min_1.default.get("/api/ImageGen/PendingJobsCount?accessKey=" + encodedAccessKey)
                .then(function (response) {
                var data = response.data;
                if (data.error != "") {
                    _this.backendStatus(_this.Truncate(data.error));
                }
                else {
                    _this.lastPendingJobs = data.count + "";
                }
            })
                .catch(function (err) {
                _this.backendStatus(_this.Truncate("Error: " + JSON.stringify(err)));
            });
        };
        ImageGenModel.prototype.QueueJob = function () {
            var _this = this;
            var encodedAccessKey = encodeURIComponent(this.accessKey());
            var encodedPrompt = encodeURIComponent(this.prompt());
            axios_0_24_0_min_1.default.post("/api/ImageGen/QueueJob?accessKey=" + encodedAccessKey + "&prompt=" + encodedPrompt)
                .then(function (response) {
                var data = response.data;
                if (data.error != "") {
                    _this.backendStatus(_this.Truncate(data.error));
                }
                else {
                    _this.jobStatus("Queued job " + data.jobId);
                    _this.backendStatus("Welcome " + data.userName);
                    // Reload current jobs after the new job was queued
                    _this.LoadJobs();
                    _this.encodedJobIdToLoad = encodeURIComponent(data.jobId);
                    _this.newImageTimer = window.setInterval(_this.TimedCheckForImage, 1000, _this);
                }
            })
                .catch(function (err) {
                _this.backendStatus(_this.Truncate("Error: " + JSON.stringify(err)));
            });
            this.UpdatePendingJobs(encodedAccessKey);
        };
        ImageGenModel.prototype.TimedCheckForImage = function (self) {
            self.imageCountdown = self.imageCountdown - 1;
            self.backendStatus("Image Generating (" + self.imageCountdown + ")");
            if (self.imageCountdown == 0) {
                var encodedAccessKey = encodeURIComponent(self.accessKey());
                self.LoadImage(encodedAccessKey, self.encodedJobIdToLoad);
                self.imageCountdown = 10;
            }
        };
        ImageGenModel.prototype.LoadImage = function (encodedAccessKey, encodedJobId) {
            var _this = this;
            axios_0_24_0_min_1.default.get("/api/ImageGen/JobResults?accessKey=" + encodedAccessKey + "&jobId=" + encodedJobId)
                .then(function (response) {
                var data = response.data;
                if (data.error != "") {
                    _this.backendStatus(_this.Truncate(data.error));
                }
                else if (data.status != "") {
                    _this.backendStatus(_this.Truncate(data.status));
                }
                else {
                    _this.backendStatus("Loaded image");
                    _this.image(data.imageData);
                    _this.UpdatePendingJobs(encodedAccessKey);
                    window.clearInterval(_this.newImageTimer);
                    _this.jobStatus(_this.selectedJob().substring(0, _this.selectedJob().length - 37));
                }
            })
                .catch(function (err) {
                _this.backendStatus(_this.Truncate("Error: " + JSON.stringify(err)));
            });
        };
        return ImageGenModel;
    }());
    var MainModel = /** @class */ (function () {
        function MainModel() {
            this.utility = new utility_1.UtilityModel();
            this.imageGen = new ImageGenModel();
        }
        return MainModel;
    }());
    ko.applyBindings(new MainModel());
});
//# sourceMappingURL=imagegen.js.map