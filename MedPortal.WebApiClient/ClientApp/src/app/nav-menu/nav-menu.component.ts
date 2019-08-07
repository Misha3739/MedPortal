import { Component } from '@angular/core';
import { SearchInfoService } from '../services/search-info-service';
import { ICity } from '../data/ICity';
import { Router, ActivatedRoute, Params, NavigationEnd } from '@angular/router';
import { Subscription } from 'rxjs';
import { GeolocationService } from '../services/geolocation-service';

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
  routeParamsSubscription: Subscription;

  constructor(
    private searchInfoService: SearchInfoService,
    private geoService: GeolocationService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit() {

    this.cities.push(this.nullCity);

    this.router.events.subscribe(val => {
      if (val instanceof NavigationEnd) {
        this.currentUrl = this.router.url;
        let splitted = this.currentUrl.split("/");
        this.cityAlias = splitted.length > 0 ? splitted[1] : null;
        console.log('NavMenuComponent', this.currentUrl, ' => ', this.cityAlias);

        if (!this.cityAlias) {
          console.log('NavMenuComponent. Start getting location for: ', this.cityAlias);
          this.geoService.getPosition().subscribe(c => {
            console.log('NavMenuComponent. Location : ', c);
            this.geoService.getCity(c.coords.latitude, c.coords.longitude).subscribe(
              cityResult => {
                console.log('NavMenuComponent. City : ', cityResult);
                this.cities = cityResult.cities;
                this.cities.unshift(this.nullCity);
                if (cityResult.current) {
                  this.city = this.cities.find(c => c.alias === cityResult.current.alias);
                  this.router.navigate([this.city.alias]);
                }
              },
              error => {
                console.log('NavMenuComponent. City error: ', error);
              }
            );
          });
        } else {
          if (!this.cities || this.cities.length <= 1) {
            this.searchInfoService.dataReceived.subscribe(res => {
              if (res === 'cities') {
                this.cities = this.searchInfoService.cities;
                this.cities.unshift(this.nullCity);
                this.city = this.cities.find(c => c.alias === this.cityAlias);
              }
            });
            this.searchInfoService.getCities();
          }
         
        }
      }
    });

   
  }

  onCityChanged() {
    console.log(this.city.name);
    if (this.city.alias === 'noCity') {
      this.router.navigate(['/']);
    } else if (!this.currentUrl || this.currentUrl === '' || this.currentUrl === '/') {
      this.router.navigate([this.city.alias]);
    } else {
      let splitted = this.currentUrl.split("/");
      splitted[1] = this.city.alias;
      let url = splitted.join('/');
      this.router.navigateByUrl(url);
    }
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  ngOnDestroy() {
    if (this.routeParamsSubscription) {
      this.routeParamsSubscription.unsubscribe();
    }
  }
}
