import { IClinic } from "./clinic";
import { ISpeciality } from "./speciality";

export interface IDoctor {
   id: number,
   surname: string;
   name: string;
   patronimic: string;
   photoUrl: string;
   experience: number;
   specialities: ISpeciality[];
   clinics: IClinic[];
}

export interface IDoctorDetails extends IDoctor {
}

