import { Injectable } from "@angular/core";
import { IDoctor,IDoctorDetails, Doctor } from "../data/doctor";

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
      id: 2,
      surname: 'Гривцова',
      name: 'Ольга',
      patronimic: 'Александровна',
      clinics: []
    });
    doctors.push({
      id: 3,
      surname: 'Салихов',
      name: 'Роберт',
      patronimic: 'Иосифович',
      clinics: []
    });
    return doctors;
  }

  getDoctor(id: number): IDoctorDetails {
    let doctor = this.getDoctors('sbp')[id - 1] as IDoctorDetails;

    return doctor;
  };
}
