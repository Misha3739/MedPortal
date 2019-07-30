import { Injectable } from "@angular/core";
import { IDoctor, Doctor } from "../data/doctor";

@Injectable()
export class DoctorsService {
  getDoctors(city: string): IDoctor[] {
    let doctors: Doctor[] = [];
    doctors.push({
      id: 1,
      surname: 'Petrov',
      name: 'Ivan',
      patronimic: 'Fedorovich',
      clinics : []
    });
    return doctors;
  }
}
