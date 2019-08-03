import { Injectable } from "@angular/core";
import { IDoctor } from "../data/doctor";
import { IClinic, IClinicDetails } from "../data/clinic";

@Injectable()
export class ClinicsService {
  private getAllClinics() {
    let clinics: IClinic[] = [];
    clinics.push({
      id: 1,
      name: 'Центр Диетологии',
      logo: 'https://cdn4.docdoc.pro/clinic/logo/min_14179.png?1562319645',
      alias: 'Center_Dietology',
      address: 'Малая Морская 22',
      latitude: 55.788895,
      longtitude: 37.614545,
      city: { id: 1, alias: 'spb', name: 'Санкт-Петербург' },
      doctors: []
    });
    clinics.push({
      id: 2,
      alias: 'Center_Prostatology',
      name: 'Центр Простатологии',
      logo: 'https://cdn4.docdoc.pro/clinic/logo/logo_default.gif?1557801558',
      address: 'Большой пр ПС 33',
      latitude: 55.756082,
      longtitude: 37.652041,
      city: { id: 1, alias: 'spb', name: 'Санкт-Петербург' },
      doctors: []
    });
    clinics.push({
      id: 3,
      alias: 'Center_FamilyMedicine',
      name: 'Центр Семейной медицины',
      logo: 'https://cdn4.docdoc.pro/clinic/logo/logo_default.gif?1562657624',
      address: 'Московский пр 143',
      latitude: 55.639425,
      longtitude: 37.65911,
      city: { id: 1, alias: 'spb', name: 'Санкт-Петербург' },
      doctors: []
    });
    clinics.push({
      id: 3,
      alias: 'Morg',
      name: 'Городской Морг',
      logo: 'https://cdn4.docdoc.pro/clinic/logo/logo_default.gif?1562657624',
      address: 'Ленина 55',
      latitude: 55.639425,
      longtitude: 37.65911,
      city: { id: 5, alias: 'ufa', name: 'Уфа' },
      doctors: []
    });
    return clinics;
  }

  getClinics(city: string): IClinic[] {
    let clinics = this.getAllClinics();
    return clinics.filter(c => c.city && c.city.alias === city);
  }

  getClinic(alias: string): IClinicDetails {
    let clinic = this.getAllClinics().find(c => c.alias === alias) as IClinicDetails;

    return clinic;
  };
}
