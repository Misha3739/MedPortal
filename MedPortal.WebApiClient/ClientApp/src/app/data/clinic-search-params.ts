import { LocationType } from "./location/location-type";

export interface IClinicSearchParams {
  city: string;
  metro?: string;
  speciality: string;
  location: { type: LocationType, alias: string }
}

