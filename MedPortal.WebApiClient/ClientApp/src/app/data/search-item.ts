import { ICity } from "./ICity";

export interface ISearchItem {
    id: number;
    alias: string;
    name: string;

    city?: ICity;
    latitude?: number;
    longitude?: number;
}
