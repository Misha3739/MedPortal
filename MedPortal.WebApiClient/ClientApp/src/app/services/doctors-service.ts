import { Injectable } from "@angular/core";
import { IDoctor,IDoctorDetails } from "../data/doctor";

@Injectable()
export class DoctorsService {
  private getAllDoctors() {
    let doctors: IDoctor[] = [];
    doctors.push({
      id: 1,
      alias: 'Petrov_Ivan',
      surname: 'Петров',
      name: 'Иван',
      patronimic: 'Федорович',
      experience: 5,
      photoUrl: 'https://cdn4.docdoc.pro/doctor/105574_20190514115235_small.jpg',
      city: { id: 1, alias: 'spb', name: 'Санкт-Петербург' },
      specialities: [{ id: 1, alias: 'Urologist', name: 'Уролог' }],
      clinics: []
    });
    doctors.push({
      id: 2,
      alias: 'Grivtsova_Olga',
      surname: 'Гривцова',
      name: 'Ольга',
      patronimic: 'Александровна',
      experience: 15,
      photoUrl: 'https://cdn4.docdoc.pro/doctor/116024_20190422214452_small.jpg',
      city: { id: 1, alias: 'spb', name: 'Санкт-Петербург' },
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
      city: { id: 5, alias: 'ufa', name: 'Уфа' },
      specialities: [{ id: 5, alias: 'Gastroenterologist', name: 'Гастроэнтеролог' }, { id: 3, alias: 'Surgeon', name: 'Хирург' }],
      clinics: []
    });
    return doctors;
  }

  getDoctors(city: string): IDoctor[] {
    let doctors = this.getAllDoctors();
    return doctors.filter(d => d.city && d.city.alias === city);
  }

  getDoctor(alias: string): IDoctorDetails {
    let doctor = this.getAllDoctors().find(d => d.alias === alias) as IDoctorDetails;

    return doctor;
  };
}
