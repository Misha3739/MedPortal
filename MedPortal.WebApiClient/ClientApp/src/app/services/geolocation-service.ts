import { Injectable, Inject } from "@angular/core";
import { ISearchCategory } from "../data/search-info";
import { SearchInfoType } from "../data/search-info-type";
import { ISpeciality } from "../data/speciality";
import { ICity } from "../data/ICity";
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Subject, Observable } from "rxjs";

@Injectable()
export class GeolocationService {
  private headers = new HttpHeaders({
    'Content-Type': 'application/json',
    //'Access-Control-Allow-Origin': '*',
    //'Access-Control-Allow-Methods': 'GET, POST, OPTIONS, DELETE, PUT',
  });

  constructor(@Inject('BASE_URL') public baseUrl: string, private httpClient: HttpClient) {
    
  }

  city: ICity;

  getPosition(): Observable<Position> {
    return Observable.create(
      (observer) => {
        navigator.geolocation.watchPosition((pos: Position) => {
          observer.next(pos);
        }),
          () => {
            console.log('Position is not available');
          },
          {
            enableHighAccuracy: true
          };
      });
  }

  getCity(latitude: number, longitude: number): Observable<any> {
    let params = new HttpParams();
    params.append("latitude", latitude.toString());
    params.append("longitude", longitude.toString());
    return this.httpClient.get(this.baseUrl + '/api/city', { headers: this.headers, params: params });
  }


    //return this.httpClient.get(this.baseUrl + '/api/city', { headers: this.headers, params: params })
    //  .subscribe((result: ICity) => {
    //    console.log('/api/city: ', result);
    //    this.city = result;
    //  });
}
