import { Injectable, Inject,PLATFORM_ID } from '@angular/core';
import {isPlatformBrowser} from '@angular/common';
import {HttpClient} from '@angular/common/http';
import { ICoordinates, ICoordinatesStorage } from '../data/location/coordinates';

@Injectable()
export class CacheService {
  static instance: CacheService;

  private _coordinates: ICoordinates;

  constructor(private http: HttpClient, @Inject(PLATFORM_ID) private platformId: Object) {
    return CacheService.instance = CacheService.instance || this;
  }

  private isBrowser(): boolean {
    return isPlatformBrowser(this.platformId);
  }

  get coordinates(): ICoordinates {
    if (this.isBrowser()) {
      let obj = localStorage.getItem('coordinates');
      if (obj) {
        let stored = JSON.parse(obj);
        try {
          let diffInMs = new Date().getTime() - Date.parse(stored.date);
          let diffInMinutes = diffInMs / 1000 / 60;
          console.log('Retrieved coordinates from storage: ', stored , diffInMinutes, ' minutes');
          if (diffInMinutes < 30) {
            this._coordinates = stored.coordinates;
          }
        }
        catch (e) {
          console.error('Unable to retreive coordinates from cache', e, stored);
        }
      }
    }
    return this._coordinates;
  }

  set coordinates(value: ICoordinates) {
    if (this.isBrowser()) {
      localStorage.setItem('coordinates', JSON.stringify({
        date: new Date(),
        coordinates: {
          latitude: value.latitude,
          longitude: value.longitude
        }
      }));
      this._coordinates = value;
    }
  }
}
