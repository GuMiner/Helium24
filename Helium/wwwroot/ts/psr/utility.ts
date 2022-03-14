import { Observable } from "../../lib/knockout-3.5.1.min";

export class UtilityModel {
    toggler: (item: Observable<string>) => void

    constructor() {
        this.toggler = (item: Observable<string>) => {
            if (item() !== "active") {
                item("active");
            } else {
                item("");
            }
        }
    }
}