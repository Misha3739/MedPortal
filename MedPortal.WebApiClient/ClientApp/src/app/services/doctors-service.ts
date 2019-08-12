import { IDoctor, IDoctorDetails } from "../data/doctor";
import { Injectable, Inject } from "@angular/core";
import { Subject } from "rxjs";
import { HttpHeaders, HttpClient, HttpParams } from "@angular/common/http";
import { IDoctorSearchParams } from "../data/doctor-search-params";
import { ISearchParams } from "../data/search-params";
import { LocationType } from "../data/location/location-type";

@Injectable()
export class DoctorsService {
  static instance: DoctorsService;

  dataReceived = new Subject();

  doctors: IDoctor[];

  private headers = new HttpHeaders({
    'Content-Type': 'application/json',
    //'Access-Control-Allow-Origin': '*',
    //'Access-Control-Allow-Methods': 'GET, POST, OPTIONS, DELETE, PUT',
  });

  constructor(@Inject('BASE_URL') public baseUrl: string, private httpClient: HttpClient) {
    return DoctorsService.instance = DoctorsService.instance || this;
  }

  private getAllDoctors() {
    let doctors: IDoctor[] = [];
    doctors.push({
      id: 1,
      originId: 1,
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
      originId: 2,
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
      originId: 3,
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

  getDoctors(params: ISearchParams) {
    let url = '/api/doctors/';
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
      .subscribe((result: IDoctor[]) => {
        console.log(url, params, result);
        this.doctors = result;
        this.dataReceived.next('doctors');
      });
  }

  getDoctorsOld(city: string): IDoctor[] {
    let doctors = this.getAllDoctors();
    return doctors.filter(d => d.city && d.city.alias === city);
  }

  getDoctor(alias: string): IDoctorDetails {
    let doctor = this.getAllDoctors().find(d => d.alias === alias) as IDoctorDetails;

    return doctor;
  };
}
