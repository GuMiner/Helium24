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
            this.jobStatus = ko.observable("");
            this.backendStatus = ko.observable("Idle");
            this.jobs = ko.observableArray(["No jobs"]);
            this.selectedJob = ko.observable("");
            this.image = ko.observable("");
            this.jobLoader = ko.computed(function () {
                if (_this.selectedJob() === undefined) {
                    return;
                }
                // Load image from the server whenever the selected job changes
                _this.jobStatus("Loading " + _this.selectedJob());
                var jobId = _this.selectedJob().split(":")[0];
                var encodedAccessKey = encodeURIComponent(_this.accessKey());
                var encodedJobId = encodeURIComponent(jobId);
                axios_0_24_0_min_1.default.get("/api/ImageGen/JobResults?accessKey=" + encodedAccessKey + "&jobId=" + encodedJobId)
                    .then(function (response) {
                    var data = response.data;
                    if (data.error != "") {
                        _this.backendStatus(data.error);
                    }
                    else if (data.status != "") {
                        _this.backendStatus(data.status);
                    }
                    else {
                        _this.backendStatus("Loaded image");
                        _this.image(data.imageData);
                    }
                })
                    .catch(function (err) {
                    _this.backendStatus("Error: " + JSON.stringify(err));
                });
            });
        }
        ImageGenModel.prototype.LoadJobs = function () {
            var _this = this;
            var encodedAccessKey = encodeURIComponent(this.accessKey());
            axios_0_24_0_min_1.default.get("/api/ImageGen/Jobs?accessKey=" + encodedAccessKey)
                .then(function (response) {
                var data = response.data;
                if (data.error != "") {
                    _this.backendStatus(data.error);
                }
                else {
                    _this.jobs(data.jobs);
                    _this.backendStatus("Welcome " + data.userName);
                }
            })
                .catch(function (err) {
                _this.backendStatus("Error: " + JSON.stringify(err));
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
                    _this.backendStatus(data.error);
                }
                else {
                    _this.jobStatus("Queued job " + data.jobId);
                    _this.backendStatus("Welcome " + data.userName);
                    // Reload current jobs after the new job was queued
                    _this.LoadJobs();
                }
            })
                .catch(function (err) {
                _this.backendStatus("Error: " + JSON.stringify(err));
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