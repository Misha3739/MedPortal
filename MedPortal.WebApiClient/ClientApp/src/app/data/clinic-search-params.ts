import { LocationType } from "./location/location-type";
import { ICoordinates } from "./location/coordinates";

export interface IClinicSearchParams {
  city: string;
  metro?: string;
  speciality: string;
  location: { type: LocationType, alias: string },
  inRange: { value: number, coordinates?: ICoordinates };
}

