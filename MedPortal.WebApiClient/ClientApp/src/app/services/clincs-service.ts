import { Injectable } from "@angular/core";
import { IDoctor } from "../data/doctor";
import { IClinic, IClinicDetails } from "../data/clinic";

@Injectable()
export class ClinicsService {
  getClinics(city: string): IClinic[] {
    let clinics: IClinic[] = [];
    clinics.push({
      id: 1,
      name: 'Центр Диетологии',
      alias: 'Center_Dietology',
      latitude: 76,
      longtitude: 66,
      doctors: []
    });
    clinics.push({
      id: 2,
      alias: 'Center_Prostatology',
      name: 'Центр Простатологии',
      latitude: 76,
      longtitude: 66,
      doctors: []
    });
    clinics.push({
      id: 3,
      alias: 'Center_FamilyMedicine',
      name: 'Центр Семейной медицины',
      latitude: 76,
      longtitude: 66,
      doctors: []
    });
    return clinics;
  }

  getClinic(alias: string): IClinicDetails {
    let clinic = this.getClinics('sbp').find(c => c.alias === alias) as IClinicDetails;

    return clinic;
  };
}
