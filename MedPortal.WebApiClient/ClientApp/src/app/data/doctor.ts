import { IClinic } from "./clinic";

export interface IDoctor {
   id: number,
   surname: string;
   name: string;
   patronimic: string;
   clinics: IClinic[];
}

export class Doctor implements IDoctor {
    id: number;
    surname: string;
    name: string;
    patronimic: string;
    clinics: IClinic[];
}
