import { IDoctor } from "./doctor";
import { ICity } from "./ICity";

export interface IClinic {
      id: number;
      alias: string;
      name: string;
      logo: string;
      address: string;
      latitude: number;
      longtitude: number;
      city: ICity;
      doctors: IDoctor[];
}

export interface IClinicDetails extends IClinic {

}
