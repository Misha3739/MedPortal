import { Injectable, Inject } from "@angular/core";
import { ISearchCategory } from "../data/search-info";
import { SearchInfoType } from "../data/search-info-type";
import { ISpeciality } from "../data/speciality";
import { ICity } from "../data/ICity";
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Subject, Observable } from "rxjs";

import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { IQuerySearchParams } from "../data/query-search-params";
import { ILocationCategory } from "../data/location/location-category";

@Injectable()
export class SearchInfoService {
  static instance: SearchInfoService;

  dataReceived = new Subject();

  cities: ICity[];
  searchInfoItems: ISearchCategory[];
  clinicSpecialities: ISpeciality[];
  doctorSpecialities: ISpeciality[];

  locationCategories: ILocationCategory[];

  private headers = new HttpHeaders({
    'Content-Type': 'application/json',
    //'Access-Control-Allow-Origin': '*',
    //'Access-Control-Allow-Methods': 'GET, POST, OPTIONS, DELETE, PUT',
  });

  constructor(@Inject('BASE_URL') public baseUrl: string, private httpClient: HttpClient) {
    return SearchInfoService.instance = SearchInfoService.instance || this;
  }

  getClinicSpecialities() {
    return this.httpClient.get(this.baseUrl + '/api/clinicspecialities', { headers: this.headers })
      .subscribe((result: ISpeciality[]) => {
        console.log('/api/clinicspecialities: ', result);
        this.clinicSpecialities = result;
        this.dataReceived.next('clinicSpecialities');
      });
  }

  getDoctorSpecialities() {
    return this.httpClient.get(this.baseUrl + '/api/doctorspecialities', { headers: this.headers })
      .subscribe((result: ISpeciality[]) => {
        console.log('/api/doctorspecialities: ', result);
        this.doctorSpecialities = result;
        this.dataReceived.next('doctorSpecialities');
      });
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

  getLocations(city: string) {
    return this.httpClient.get(this.baseUrl + '/api/locations/' + city, { headers: this.headers })
      .subscribe((result: ILocationCategory[]) => {
        console.log('/api/locations: ', result);
        this.locationCategories = result;
        this.dataReceived.next('locations');
      });;
  }

  search(terms: Observable<IQuerySearchParams>) {
    return terms.pipe(
      debounceTime(400),
      distinctUntilChanged(),
      switchMap(term => this.searchEntries(term)));
  }

  searchEntries(term: IQuerySearchParams) {
    let httpParams = new HttpParams();
    if (term.city) {
      httpParams = httpParams.set('city', term.city);
    }
    httpParams = httpParams.set('query', term.query);
    console.log('SearchInfoService. Getting result for ', term.city, ' ', term.query);
    return this.httpClient
      .get(this.baseUrl + '/api/search', { params: httpParams });
  }
}
