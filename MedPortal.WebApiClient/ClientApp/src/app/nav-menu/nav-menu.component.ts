import { Component } from '@angular/core';
import { SearchInfoService } from '../services/search-info-service';
import { ICity } from '../data/ICity';
import { Router, ActivatedRoute, Params, NavigationEnd } from '@angular/router';
import { Subscription } from 'rxjs';
import { GeolocationService } from '../services/geolocation-service';
import { UrlQueryParameters } from '../data/constants/url-query-parameters';
import { CacheService } from '../services/cache-service';
import { ICoordinates } from '../data/location/coordinates';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;

  cities: ICity[] = [];

  nullCity: ICity = { alias: 'noCity', name: 'Не выбран', id: 0 };
  city: ICity = this.nullCity;

  currentUrl: string = null;
  cityAlias: string;

  private routerParamsSubscription: Subscription;
  private queryParamsSubscription: Subscription;
  private geoServiceSubscription: Subscription;

  queryParameters: any = {};

  constructor(
    private searchInfoService: SearchInfoService,
    private geoService: GeolocationService,
    private route: ActivatedRoute,
    private router: Router,
    private cacheService: CacheService) { }

  ngOnInit() {

    this.cities.push(this.nullCity);

    this.routerParamsSubscription = this.router.events.subscribe(val => {
      if (val instanceof NavigationEnd) {
        this.currentUrl = this.router.url;
        let splitted = this.currentUrl.split("/");
        this.cityAlias = splitted.length > 0 ? splitted[1] : null;
        console.log('NavMenuComponent', this.currentUrl, ' => ', this.cityAlias);

        let cahchedCoordinates: ICoordinates = this.cacheService.coordinates;
        if (cahchedCoordinates) {
          this.getCities(cahchedCoordinates);
        } else {
          this.geoServiceSubscription = this.geoService.getPosition().subscribe(c => {
            console.log('NavMenuComponent. Location : ', c);
            this.cacheService.coordinates = c.coords;
            this.getCities(c.coords);
          });
        }
      }
    });

    this.queryParamsSubscription = this.queryParamsSubscription = this.route.queryParamMap.subscribe(params => {
      this.queryParameters[UrlQueryParameters.SPECIALITY] = params.get(UrlQueryParameters.SPECIALITY);
      this.queryParameters[UrlQueryParameters.LOCATIONTYPE] = +params.get(UrlQueryParameters.LOCATIONTYPE);
      this.queryParameters[UrlQueryParameters.LOCATION] = params.get(UrlQueryParameters.LOCATION);
      this.queryParameters[UrlQueryParameters.INRANGE] = +params.get(UrlQueryParameters.INRANGE);
    });


  }

  getCities(coords: ICoordinates) {
    this.geoService.getCity(coords.latitude, coords.longitude).subscribe(
      cityResult => {
        console.log('NavMenuComponent. City : ', cityResult);
        this.cities = cityResult.cities;
        this.cities.unshift(this.nullCity);
        if (this.cityAlias) {
          this.city = this.cities.find(c => c.alias === this.cityAlias);
        } else if (!this.cityAlias && cityResult.current) {
          this.city = this.cities.find(c => c.alias === cityResult.current.alias);
          this.router.navigate([this.city.alias]);
        }
      },
      error => {
        console.log('NavMenuComponent. City error: ', error);
      }
    );

  }

  onCityChanged() {
    console.log(this.city.name);
    if (this.city.alias === 'noCity') {
      this.router.navigate(['/']);
    } else if (!this.currentUrl || this.currentUrl === '' || this.currentUrl === '/') {
      this.router.navigate([this.city.alias]);
    } else {
      let splitted = this.currentUrl.split('/');
      splitted[1] = this.city.alias;
      //Replace query parameters
      let last = splitted[splitted.length - 1];
      const questionIndex = last.indexOf('?');
      if (questionIndex && questionIndex !== -1) {
        last = last.substring(0, questionIndex);
      }
      
      splitted[splitted.length - 1] = last;
      let url = splitted.join('/');

      let queryParams: any = {};
      Object.keys(this.queryParameters).forEach(key => {
        if (key !== UrlQueryParameters.LOCATIONTYPE && key !== UrlQueryParameters.LOCATION) {
          if (this.queryParameters[key]) {
            queryParams[key] = this.queryParameters[key];
          }
        }
      });

      this.router.navigate([url], {queryParams: queryParams, queryParamsHandling: null });
    }
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  onDoctorsNavigate() {
    if (this.city && this.city.alias && this.city.alias !== 'noCity') {
      this.router.navigate([this.city.alias + "/doctors"]);
    }
  }

  onClinicsNavigate() {
    if (this.city && this.city.alias && this.city.alias !== 'noCity') {
      this.router.navigate([this.city.alias + "/clinics"]);
    }
  }

  ngOnDestroy() {
    if (this.routerParamsSubscription) {
      this.routerParamsSubscription.unsubscribe();
    }
    if (this.queryParamsSubscription) {
      this.queryParamsSubscription.unsubscribe();
    }

    if (this.geoServiceSubscription) {
      this.geoServiceSubscription.unsubscribe();
    }
  }
}
