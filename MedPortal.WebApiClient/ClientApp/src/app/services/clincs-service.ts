import { Injectable } from "@angular/core";
import { IDoctor } from "../data/doctor";
import { IClinic, Clinic, IClinicDetails } from "../data/clinic";

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

  getClinic(id: number): IClinicDetails {
    let clinic = this.getClinics('sbp')[id-1] as IClinicDetails;

    return clinic;
  };
}
