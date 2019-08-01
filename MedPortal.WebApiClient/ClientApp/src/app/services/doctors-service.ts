import { Injectable } from "@angular/core";
import { IDoctor,IDoctorDetails } from "../data/doctor";

@Injectable()
export class DoctorsService {
  getDoctors(city: string): IDoctor[] {
    let doctors: IDoctor[] = [];
    doctors.push({
      id: 1,
      alias: 'Petrov_Ivan',
      surname: 'Петров',
      name: 'Иван',
      patronimic: 'Федорович',
      experience: 5,
      photoUrl: 'https://cdn4.docdoc.pro/doctor/105574_20190514115235_small.jpg',
      specialities: [{id : 1, alias: 'Urologist', name : 'Уролог'}],
      clinics : []
    });
    doctors.push({
      id: 2,
      alias: 'Grivtsova_Olga',
      surname: 'Гривцова',
      name: 'Ольга',
      patronimic: 'Александровна',
      experience: 15,
      photoUrl: 'https://cdn4.docdoc.pro/doctor/116024_20190422214452_small.jpg',
      specialities: [{ id: 2, alias: 'Cardiologist', name: 'Кардиолог' }, { id: 3, alias: 'Surgeon', name: 'Хирург' }],
      clinics: []
    });
    doctors.push({
      id: 3,
      alias: 'Salikhov_Robert',
      surname: 'Салихов',
      name: 'Роберт',
      patronimic: 'Иосифович',
      experience: 25,
      photoUrl: 'https://cdn4.docdoc.pro/doctor/101008_small.jpg',
      specialities: [{ id: 5, alias: 'Gastroenterologist', name: 'Гастроэнтеролог' }, { id: 3, alias: 'Surgeon', name: 'Хирург' }],
      clinics: []
    });
    return doctors;
  }

  getDoctor(alias: string): IDoctorDetails {
    let doctor = this.getDoctors('sbp').find(d => d.alias === alias) as IDoctorDetails;

    return doctor;
  };
}
