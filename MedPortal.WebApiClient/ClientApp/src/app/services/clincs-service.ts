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
      logo: 'https://cdn4.docdoc.pro/clinic/logo/min_14179.png?1562319645',
      alias: 'Center_Dietology',
      address:'Малая Морская 22',
      latitude: 76,
      longtitude: 66,
      doctors: []
    });
    clinics.push({
      id: 2,
      alias: 'Center_Prostatology',
      name: 'Центр Простатологии',
      logo: 'https://cdn4.docdoc.pro/clinic/logo/logo_default.gif?1557801558',
      address: 'Большой пр ПС 33',
      latitude: 76,
      longtitude: 66,
      doctors: []
    });
    clinics.push({
      id: 3,
      alias: 'Center_FamilyMedicine',
      name: 'Центр Семейной медицины',
      logo: 'https://cdn4.docdoc.pro/clinic/logo/logo_default.gif?1562657624',
      address: 'Московский пр 143',
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
