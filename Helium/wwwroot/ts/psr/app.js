var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
define(["require", "exports", "../../lib/axios-0.24.0.min", "../../lib/knockout-3.5.1.min", "./utility"], function (require, exports, axios_0_24_0_min_1, ko, utility_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    axios_0_24_0_min_1 = __importDefault(axios_0_24_0_min_1);
    var SubstitutionModel = /** @class */ (function () {
        function SubstitutionModel() {
            var _this = this;
            this.input = ko.observable("");
            this.letterToNumberActive = ko.observable("active");
            this.rotate13Active = ko.observable("active");
            this.rotateValue = ko.observable("13");
            this.aciiActive = ko.observable("active");
            this.morseActive = ko.observable("");
            this.delimiter = ko.observable(" ");
            this.morseDot = ko.observable(".");
            this.morseDash = ko.observable("-");
            this.output = ko.pureComputed(function () {
                var outputString = "";
                var inputText = _this.input();
                var parts = inputText.split(_this.delimiter());
                if (_this.letterToNumberActive()) {
                    outputString += SubstitutionModel.letterToNumber(parts) + "\r\n";
                }
                if (_this.rotate13Active()) {
                    outputString += SubstitutionModel.rot13(parts, _this.rotateValue()) + "\r\n";
                }
                if (_this.aciiActive()) {
                    outputString += SubstitutionModel.numberToAscii(parts) + "\r\n";
                }
                if (_this.morseActive()) {
                    outputString += SubstitutionModel.convertMorse(parts, _this.morseDot(), _this.morseDash()) + "\r\n";
                }
                return outputString;
            }, this);
        }
        SubstitutionModel.letterToNumber = function (parts) {
            var a1z26String = "";
            for (var i = 0; i < parts.length; i++) {
                var integer = (parseInt(parts[i]) - 1) % 26;
                if (isNaN(integer)) {
                    a1z26String += parts[i];
                }
                else {
                    a1z26String += String.fromCharCode(65 + integer);
                }
                a1z26String += " ";
            }
            return a1z26String;
        };
        SubstitutionModel.rot13 = function (parts, rotateAmount) {
            var rot13String = "";
            for (var i = 0; i < parts.length; i++) {
                if (parts[i].length == 1) {
                    var charValue = parts[i].toUpperCase().charCodeAt(0);
                    charValue += parseInt(rotateAmount);
                    if (charValue > "Z".charCodeAt(0)) {
                        charValue -= 26;
                    }
                    rot13String += String.fromCharCode(charValue);
                }
                else {
                    rot13String += parts[i];
                }
                rot13String += " ";
            }
            return rot13String;
        };
        SubstitutionModel.numberToAscii = function (parts) {
            var asciiString = "";
            for (var i = 0; i < parts.length; i++) {
                var integer = parseInt(parts[i]);
                if (isNaN(integer)) {
                    asciiString += parts[i];
                }
                else {
                    asciiString += String.fromCharCode(integer);
                }
                asciiString += " ";
            }
            return asciiString;
        };
        SubstitutionModel.convertMorse = function (parts, dotChar, dashChar) {
            var morseString = "";
            for (var i = 0; i < parts.length; i++) {
                var letter = SubstitutionModel.parseMorseCharacter(parts[i], dotChar, dashChar);
                morseString += letter;
            }
            return morseString;
        };
        // https://stackoverflow.com/questions/3446170/escape-string-for-use-in-javascript-regex
        SubstitutionModel.escapeRegExp = function (input) {
            return input.replace(/[.*+?^${}()|[\]\\]/g, '\\$&'); // $& means the whole matched string
        };
        SubstitutionModel.parseMorseCharacter = function (character, dotChar, dashChar) {
            // From https://en.wikipedia.org/wiki/Morse_code#/media/File:International_Morse_Code.svg
            var dotCharRegex = new RegExp(SubstitutionModel.escapeRegExp(dotChar), "g");
            var dashCharRegex = new RegExp(SubstitutionModel.escapeRegExp(dashChar), "g");
            var normalizedCharacter = character.replace(dotCharRegex, ".").replace(dashCharRegex, "-");
            switch (normalizedCharacter) {
                case ".-": return "A";
                case "-...": return "B";
                case "-.-.": return "C";
                case "-..": return "D";
                case ".": return "E";
                case "..-.": return "F";
                case "--.": return "G";
                case "....": return "H";
                case "..": return "I";
                case ".---": return "J";
                case "-.-": return "K";
                case ".-..": return "L";
                case "--": return "M";
                case "-.": return "N";
                case "---": return "O";
                case ".--.": return "P";
                case "--.-": return "Q";
                case ".-.": return "R";
                case "...": return "S";
                case "-": return "T";
                case "..-": return "U";
                case "...-": return "V";
                case ".--": return "W";
                case "-..-": return "X";
                case "-.--": return "Y";
                case "--..": return "Z";
                case ".----": return "1";
                case "..---": return "2";
                case "...--": return "3";
                case "....-": return "4";
                case ".....": return "5";
                case "-....": return "6";
                case "--...": return "7";
                case "---..": return "8";
                case "----.": return "9";
                case "-----": return "0";
                default: return "?";
            }
        };
        return SubstitutionModel;
    }());
    var WordSearchModel = /** @class */ (function () {
        function WordSearchModel() {
            var _this = this;
            this.query = ko.observable("");
            this.searchType = ko.observable(true);
            this.resultCount = ko.observable("0");
            this.dbStatus = ko.observable("Idler");
            this.output = ko.observable("");
            this.outputProcessor = ko.computed(function () {
                _this.dbStatus("Querying...");
                var encodedQuery = encodeURIComponent(_this.query());
                if (_this.searchType()) { // == search
                    axios_0_24_0_min_1.default.get("/api/WordSearch/FindMatchingWords?search=" + encodedQuery)
                        .then(function (response) {
                        var data = response.data;
                        _this.ApplySearchResults(data);
                    })
                        .catch(function (err) {
                        _this.dbStatus("Error: " + JSON.stringify(err));
                    });
                }
                else {
                    axios_0_24_0_min_1.default.get("/api/WordSearch/FindAnagrams?search=" + encodedQuery)
                        .then(function (response) {
                        var data = response.data;
                        _this.ApplySearchResults(data);
                    })
                        .catch(function (err) {
                        _this.dbStatus("Error: " + JSON.stringify(err));
                    });
                }
                return encodedQuery;
            });
        }
        WordSearchModel.prototype.ApplySearchResults = function (data) {
            if (data.count < 0) {
                this.dbStatus("Query error: " + data.errorMessage);
            }
            else {
                this.dbStatus("Idle");
                this.resultCount(this.GetResultCountText(data.count));
                this.output(data.results.join("\n"));
            }
        };
        WordSearchModel.prototype.GetResultCountText = function (resultCount) {
            if (resultCount >= 200) {
                return resultCount.toString() + " (limited!)";
            }
            return resultCount.toString();
        };
        return WordSearchModel;
    }());
    var CrosswordSearchModel = /** @class */ (function () {
        function CrosswordSearchModel() {
            var _this = this;
            this.query = ko.observable("");
            this.resultCount = ko.observable("0");
            this.dbStatus = ko.observable("Idler");
            this.clueOutput = ko.observable("");
            this.answerOutput = ko.observable("");
            this.outputProcessor = ko.computed(function () {
                _this.dbStatus("Querying...");
                var encodedQuery = encodeURIComponent(_this.query());
                axios_0_24_0_min_1.default.get("/api/CrosswordSearch/FindMatchingWords?search=" + encodedQuery)
                    .then(function (response) {
                    var data = response.data;
                    if (data.count < 0) {
                        _this.dbStatus("Query error: " + data.errorMessage);
                    }
                    else {
                        _this.dbStatus("Idle");
                        _this.resultCount(_this.GetResultCountText(data.count));
                        _this.clueOutput(data.clueResults.join("\n"));
                        _this.answerOutput(data.answerResults.join("\n"));
                    }
                })
                    .catch(function (err) {
                    _this.dbStatus("Error: " + JSON.stringify(err));
                });
                return encodedQuery;
            });
        }
        CrosswordSearchModel.prototype.GetResultCountText = function (resultCount) {
            if (resultCount >= 400) {
                return resultCount.toString() + " (limited!)";
            }
            return resultCount.toString();
        };
        return CrosswordSearchModel;
    }());
    var WordExtraModel = /** @class */ (function () {
        function WordExtraModel() {
            var _this = this;
            this.query = ko.observable("");
            this.searchType = ko.observable("thesaurus");
            this.resultCount = ko.observable("0");
            this.dbStatus = ko.observable("Idler");
            this.output = ko.observable("");
            this.outputProcessor = ko.computed(function () {
                _this.dbStatus("Querying...");
                var encodedQuery = encodeURIComponent(_this.query());
                if (_this.searchType() === "thesaurus") {
                    axios_0_24_0_min_1.default.get("/api/WordExtra/FindSynonyms?search=" + encodedQuery)
                        .then(function (response) {
                        var data = response.data;
                        _this.ApplySearchResults(data, 10);
                    })
                        .catch(function (err) {
                        _this.dbStatus("Error: " + JSON.stringify(err));
                    });
                }
                else { // Homophones
                    axios_0_24_0_min_1.default.get("/api/WordExtra/FindHomophones?search=" + encodedQuery)
                        .then(function (response) {
                        var data = response.data;
                        _this.ApplySearchResults(data, 50);
                    })
                        .catch(function (err) {
                        _this.dbStatus("Error: " + JSON.stringify(err));
                    });
                }
                return encodedQuery;
            });
        }
        WordExtraModel.prototype.ApplySearchResults = function (data, throttleLimit) {
            if (data.count < 0) {
                this.dbStatus("Query error: " + data.errorMessage);
            }
            else {
                this.dbStatus("Idle");
                this.resultCount(this.GetResultCountText(data.count, throttleLimit));
                this.output(data.results.join("\n"));
            }
        };
        WordExtraModel.prototype.GetResultCountText = function (resultCount, throttleLimit) {
            if (resultCount >= throttleLimit) {
                return resultCount.toString() + " (limited!)";
            }
            return resultCount.toString();
        };
        return WordExtraModel;
    }());
    var EquationSolverModel = /** @class */ (function () {
        function EquationSolverModel() {
            var _this = this;
            this.input = ko.observable("");
            this.delimiter = ko.observable(" ");
            this.inputBase = ko.observable("10");
            this.output = ko.pureComputed(function () {
                var outputString = "";
                var base = parseInt(_this.inputBase());
                var delimiter = _this.delimiter();
                var parts = _this.input().split(delimiter);
                for (var j = 2; j <= 36; j++) {
                    // Header
                    var convertedNumbers = "(" + j + ") ";
                    if (j < 10) {
                        convertedNumbers += " ";
                    }
                    // Convert values (read as base, output to base 'j')
                    for (var i = 0; i < parts.length; i++) {
                        var number = parseInt(parts[i], base);
                        convertedNumbers += (number.toString(j) + delimiter);
                    }
                    outputString += convertedNumbers + "\r\n";
                }
                return outputString;
            });
        }
        return EquationSolverModel;
    }());
    var MainModel = /** @class */ (function () {
        function MainModel() {
            this.utility = new utility_1.UtilityModel();
            this.subst = new SubstitutionModel();
            this.wordSearch = new WordSearchModel();
            this.crosswordSearch = new CrosswordSearchModel();
            this.wordExtra = new WordExtraModel();
            this.equationSolver = new EquationSolverModel();
        }
        return MainModel;
    }());
    ko.applyBindings(new MainModel());
});
//# sourceMappingURL=app.js.map