import { Injectable } from "@angular/core";
import { IDoctor, Doctor } from "../data/doctor";
import { IClinic, Clinic } from "../data/clinic";

@Injectable()
export class ClinicsService {
  getClinics(city: string): IClinic[] {
    let clinics: Clinic[] = [];
    clinics.push({
      id: 1,
      name: 'Центр Диетологии',
      latitude: 76,
      longtitude: 66,
      doctors: []
    });
    clinics.push({
      id: 2,
      name: 'Центр Простатологии',
      latitude: 76,
      longtitude: 66,
      doctors: []
    });
    clinics.push({
      id: 2,
      name: 'Центр Семейной медицины',
      latitude: 76,
      longtitude: 66,
      doctors: []
    });
    return clinics;
  }

  getClinic(id: number): IClinic {
    return {
      id: 1,
      name: 'Центр Диетологии',
      latitude: 76,
      longtitude: 66,
      doctors: [
        {
          id: 1,
          surname: 'Петров',
          name: 'Иван',
          patronimic: 'Федорович',
          clinics: []
        },
        {
          id: 2,
          surname: 'Гривцова',
          name: 'Ольга',
          patronimic: 'Александровна',
          clinics: []
        }
      ]
    }
  };
}
