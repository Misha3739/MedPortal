import { LocationType } from "./location-type";
import { ILocationObject } from "./location-object";

export interface ILocationCategory {
    type: LocationType;
    items: ILocationObject[];
}
