import { Injectable } from "@angular/core";
import { IDoctor, Doctor } from "../data/doctor";

@Injectable()
export class DoctorsService {
  getDoctors(city: string): IDoctor[] {
    let doctors: Doctor[] = [];
    doctors.push({
      id: 1,
      surname: 'Петров',
      name: 'Иван',
      patronimic: 'Федорович',
      clinics : []
    });
    doctors.push({
      id: 1,
      surname: 'Гривцова',
      name: 'Ольга',
      patronimic: 'Александровна',
      clinics: []
    });
    doctors.push({
      id: 1,
      surname: 'Салихов',
      name: 'Роберт',
      patronimic: 'Иосифович',
      clinics: []
    });
    return doctors;
  }
}
