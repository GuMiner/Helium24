define(["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.UtilityModel = void 0;
    var UtilityModel = /** @class */ (function () {
        function UtilityModel() {
            this.toggler = function (item) {
                if (item() !== "active") {
                    item("active");
                }
                else {
                    item("");
                }
            };
        }
        return UtilityModel;
    }());
    exports.UtilityModel = UtilityModel;
});
//# sourceMappingURL=utility.js.map