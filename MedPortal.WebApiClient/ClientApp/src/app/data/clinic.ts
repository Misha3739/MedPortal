import { IDoctor } from "./doctor";

export interface IClinic {
      id: number,
      name: string,
      latitude: number,
      longtitude: number,
      doctors: IDoctor[];
}

export interface IClinicDetails extends IClinic {

}

export class Clinic implements IClinic {
    id: number;
    name: string;
    latitude: number;
    longtitude: number;
    doctors: IDoctor[];
}
