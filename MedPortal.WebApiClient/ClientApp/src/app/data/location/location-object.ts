import { LocationType } from '../../data/location/location-type';
export interface ILocationObject {
    id: number;
    alias: string;
    name: string;
    latitude?: number;
    longitude?: number;
    type: LocationType;
}
