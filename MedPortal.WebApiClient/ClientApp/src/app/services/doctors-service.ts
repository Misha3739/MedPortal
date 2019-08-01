import { Injectable } from "@angular/core";
import { IDoctor,IDoctorDetails } from "../data/doctor";

@Injectable()
export class DoctorsService {
  getDoctors(city: string): IDoctor[] {
    let doctors: IDoctor[] = [];
    doctors.push({
      id: 1,
      surname: 'Петров',
      name: 'Иван',
      patronimic: 'Федорович',
      experience: 5,
      photoUrl: 'https://cdn4.docdoc.pro/doctor/105574_20190514115235_small.jpg',
      specialities: [{id : 1, name : 'Уролог'}],
      clinics : []
    });
    doctors.push({
      id: 2,
      surname: 'Гривцова',
      name: 'Ольга',
      patronimic: 'Александровна',
      experience: 15,
      photoUrl: 'https://cdn4.docdoc.pro/doctor/116024_20190422214452_small.jpg',
      specialities: [{ id: 2, name: 'Кардиолог' },{ id: 3, name: 'Хирург' }],
      clinics: []
    });
    doctors.push({
      id: 3,
      surname: 'Салихов',
      name: 'Роберт',
      patronimic: 'Иосифович',
      experience: 25,
      photoUrl: 'https://cdn4.docdoc.pro/doctor/101008_small.jpg',
      specialities: [{ id: 5, name: 'Гастроэнтеролог' }, { id: 3, name: 'Хирург' }],
      clinics: []
    });
    return doctors;
  }

  getDoctor(id: number): IDoctorDetails {
    let doctor = this.getDoctors('sbp')[id - 1] as IDoctorDetails;

    return doctor;
  };
}
