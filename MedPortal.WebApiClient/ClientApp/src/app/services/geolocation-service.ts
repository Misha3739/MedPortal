import { Injectable, Inject } from "@angular/core";
import { ISearchCategory } from "../data/search-info";
import { SearchInfoType } from "../data/search-info-type";
import { ISpeciality } from "../data/speciality";
import { ICity } from "../data/ICity";
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Subject, Observable, Subscription } from "rxjs";
import { ICoordinates } from "../data/location/coordinates";

@Injectable()
export class GeolocationService {
  static instance: GeolocationService;
  private headers = new HttpHeaders({
    'Content-Type': 'application/json',
    //'Access-Control-Allow-Origin': '*',
    //'Access-Control-Allow-Methods': 'GET, POST, OPTIONS, DELETE, PUT',
  });

  constructor(@Inject('BASE_URL') public baseUrl: string, private httpClient: HttpClient) {
    return GeolocationService.instance = GeolocationService.instance || this;
  }

  city: ICity;
  currentPosition: ICoordinates = { latitude: 0, longitude: 0 };
  currentPositionChanged = new Subject();

  getPosition(): Observable<Position> {
    return Observable.create(
      (observer) => {
        navigator.geolocation.watchPosition((pos: Position) => {
          console.log('GeolocationService. Position received: ', pos);
          this.currentPosition = {
            latitude: pos.coords.latitude,
            longitude: pos.coords.longitude
          };
          observer.next(pos);
          this.currentPositionChanged.next();
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
    let params = new HttpParams()
      .set("latitude", latitude.toString())
      .set("longitude", longitude.toString());
    return this.httpClient.get(this.baseUrl + '/api/city', { headers: this.headers, params: params });
  }
}
