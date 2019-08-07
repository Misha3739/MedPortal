import { Injectable, Inject } from "@angular/core";
import { ISearchCategory } from "../data/search-info";
import { SearchInfoType } from "../data/search-info-type";
import { ISpeciality } from "../data/speciality";
import { ICity } from "../data/ICity";
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Subject } from "rxjs";

@Injectable()
export class SearchInfoService {
  static instance: SearchInfoService;

  dataReceived = new Subject();

  cities: ICity[];
  searchInfoItems: ISearchCategory[];
  clinicSpecialities: ISpeciality[];

  private headers = new HttpHeaders({
    'Content-Type': 'application/json',
    //'Access-Control-Allow-Origin': '*',
    //'Access-Control-Allow-Methods': 'GET, POST, OPTIONS, DELETE, PUT',
  });

  constructor(@Inject('BASE_URL') public baseUrl: string, private httpClient: HttpClient) {
    return SearchInfoService.instance = SearchInfoService.instance || this;
  }

  getSearchInfoOld(city: string): ISearchCategory[] {
    let searchInfo: ISearchCategory[] = [];
    searchInfo.push({
      type: SearchInfoType.clinic,
      items: [
        {
          id: 1,
          name: 'Центр Диетологии',
          alias: 'Center_Dietology'
        },
        {
          id: 2,
          alias: 'Center_Prostatology',
          name: 'Центр Простатологии'
        },
        {
          id: 3,
          alias: 'Center_FamilyMedicine',
          name: 'Центр Семейной медицины'
        }
      ]
    });
    searchInfo.push({
      type: SearchInfoType.doctor,
      items: [
        { id: 1, alias: 'Petrov_Ivan', name: 'Петров Иван Федорович' },
        { id: 2, alias: 'Grivtsova_Olga', name: 'Гривцова Ольга Александровна' },
        { id: 3, alias: 'Salikhov_Robert', name: 'Салихов Роберт Иосифович' }
      ]
    });
    searchInfo.push({
      type: SearchInfoType.doctorSpeciality,
      items: [
        { id: 1, alias:'urologist', name: 'Уролог' },
        { id: 2, alias: 'surgeon', name: 'Хирург' },
        { id: 3, alias: 'ginecologist', name: 'Гинеколог' }]
    });

    return searchInfo;
  }

  getClinicSpecialities() {
    return this.httpClient.get(this.baseUrl + '/api/clinicspecialities', { headers: this.headers })
      .subscribe((result: ISpeciality[]) => {
        console.log('/api/clinicspecialities: ', result);
        this.clinicSpecialities = result;
        this.dataReceived.next('specialities');
      });
  }

  getSpecialitiesOld(): ISpeciality[] {
    return [
      { id: 1, alias: 'Urologist', name: 'Уролог' },
      { id: 2, alias: 'Cardiologist', name: 'Кардиолог' },
      { id: 5, alias: 'Gastroenterologist', name: 'Гастроэнтеролог' },
      { id: 3, alias: 'Surgeon', name: 'Хирург' }];
  }

  getSearchInfo(city: string) {
    let url = '/api/searchItems/';
    if (city) {
      url += city;
    }
    return this.httpClient.get(this.baseUrl + url, { headers: this.headers })
      .subscribe((result: ISearchCategory[]) => {
        console.log('${url}: ', result);
        this.searchInfoItems = result;
        this.dataReceived.next('searchItems');
      });
  }

  getCities() {
    return this.httpClient.get(this.baseUrl + '/api/cities', { headers: this.headers})
      .subscribe((result: ICity[]) => {
        console.log('/api/cities: ', result);
        this.cities = result;
        this.dataReceived.next('cities');
      });
  }

  getCitiesOld(): ICity[] {
    return [
      { id: 1, alias: 'spb', name: 'Санкт-Петербург' },
      { id: 2, alias: 'moscow', name: 'Москва' },
      { id: 3, alias: 'voronezh', name: 'Воронеж' },
      { id: 5, alias: 'ufa', name: 'Уфа' }];
  }
}
