import { IDoctor } from "./doctor";

export interface IClinic {
      id: number,
      alias: string,
      name: string,
      logo: string;
      address: string,
      latitude: number,
      longtitude: number,
      doctors: IDoctor[];
}

export interface IClinicDetails extends IClinic {

}
