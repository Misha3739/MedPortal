import { Injectable, Inject } from "@angular/core";
import { IDoctor } from "../data/doctor";
import { IClinic, IClinicDetails } from "../data/clinic";
import { Subject } from "rxjs";
import { HttpHeaders, HttpClient, HttpParams } from "@angular/common/http";
import { ISearchParams } from "../data/search-params";
import { LocationType } from "../data/location/location-type";

@Injectable()
export class ClinicsService {

  static instance: ClinicsService;

  dataReceived = new Subject();

  clinics: IClinic[];

  private headers = new HttpHeaders({
    'Content-Type': 'application/json',
    //'Access-Control-Allow-Origin': '*',
    //'Access-Control-Allow-Methods': 'GET, POST, OPTIONS, DELETE, PUT',
  });

  constructor(@Inject('BASE_URL') public baseUrl: string, private httpClient: HttpClient) {
    return ClinicsService.instance = ClinicsService.instance || this;
  }

  private getAllClinics() {
    let clinics: IClinic[] = [];
    clinics.push({
      id: 1,
      originId: 1,
      name: 'Центр Диетологии',
      logo: 'https://cdn4.docdoc.pro/clinic/logo/min_14179.png?1562319645',
      alias: 'Center_Dietology',
      address: 'Малая Морская 22',
      latitude: 55.788895,
      longitude: 37.614545,
      city: { id: 1, alias: 'spb', name: 'Санкт-Петербург' },
      doctors: []
    });
    clinics.push({
      id: 2,
      originId: 2,
      alias: 'Center_Prostatology',
      name: 'Центр Простатологии',
      logo: 'https://cdn4.docdoc.pro/clinic/logo/logo_default.gif?1557801558',
      address: 'Большой пр ПС 33',
      latitude: 55.756082,
      longitude: 37.652041,
      city: { id: 1, alias: 'spb', name: 'Санкт-Петербург' },
      doctors: []
    });
    clinics.push({
      id: 3,
      originId: 3,
      alias: 'Center_FamilyMedicine',
      name: 'Центр Семейной медицины',
      logo: 'https://cdn4.docdoc.pro/clinic/logo/logo_default.gif?1562657624',
      address: 'Московский пр 143',
      latitude: 55.639425,
      longitude: 37.65911,
      city: { id: 1, alias: 'spb', name: 'Санкт-Петербург' },
      doctors: []
    });
    clinics.push({
      id: 3,
      originId: 3,
      alias: 'Morg',
      name: 'Городской Морг',
      logo: 'https://cdn4.docdoc.pro/clinic/logo/logo_default.gif?1562657624',
      address: 'Ленина 55',
      latitude: 55.639425,
      longitude: 37.65911,
      city: { id: 5, alias: 'ufa', name: 'Уфа' },
      doctors: []
    });
    return clinics;
  }

  getClinics(params: ISearchParams) {
    let url = '/api/clinics/';
    let httpParams = new HttpParams();
    if (params.city) {
      httpParams = httpParams.set('city', params.city);
    }
    if (params.speciality) {
      httpParams = httpParams.set('speciality', params.speciality);
    }

    if (params.location.type !== LocationType.none) {
      httpParams = httpParams.set('locationType', params.location.type.toString());
      httpParams = httpParams.set('location', params.location.alias);
    }

    if (params.inRange.value > 0 && params.inRange.coordinates) {
      httpParams = httpParams.set('inrange', params.inRange.value.toString());
      httpParams = httpParams.set('latitude', params.inRange.coordinates.latitude.toString());
      httpParams = httpParams.set('longitude', params.inRange.coordinates.longitude.toString());
    }
    
    return this.httpClient.get(this.baseUrl + url, { headers: this.headers, params: httpParams })
      .subscribe((result: IClinic[]) => {
        console.log('Clinic service request completed : ', url, ' Params: ', params, ' Result: ', result);
        this.clinics = result;
        this.dataReceived.next('clinics');
      });
  }


  getClinic(alias: string): IClinicDetails {
    let clinic = this.getAllClinics().find(c => c.alias === alias) as IClinicDetails;

    return clinic;
  };
}
