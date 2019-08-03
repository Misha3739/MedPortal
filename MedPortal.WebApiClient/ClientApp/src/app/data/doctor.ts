import { IClinic } from "./clinic";
import { ISpeciality } from "./speciality";
import { ICity } from "./ICity";

export interface IDoctor {
   id: number,
   alias: string,
   surname: string;
   name: string;
   patronimic: string;
   photoUrl: string;
   experience: number;
   city: ICity;
   specialities: ISpeciality[];
   clinics: IClinic[];
}

export interface IDoctorDetails extends IDoctor {
}

