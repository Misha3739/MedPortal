import { Component } from '@angular/core';
import { SearchInfoService } from '../services/search-info-service';
import { ICity } from '../data/ICity';
import { Router, ActivatedRoute, Params, NavigationEnd } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;

  cities: ICity[];

  nullCity: ICity = { alias: 'noCity', name: 'Не выбран', id: 0 };
  city: ICity = this.nullCity;

  currentUrl: string = null;
  cityAlias: string;
  routeParamsSubscription: Subscription;

  constructor(private searchInfoService: SearchInfoService, private route: ActivatedRoute, private router: Router) { }

  ngOnInit() {
    this.searchInfoService.getCities();

    this.searchInfoService.dataReceived.subscribe(res => {
      this.cities = this.searchInfoService.cities;
      this.cities.unshift(this.nullCity);

      if (this.cityAlias && this.cityAlias !== '') {
        this.city = this.cities.find(c => c.alias === this.cityAlias);
        ToDo: //Find by gmaps
        if (!this.city) {
          this.city = this.cities.find(c => c.alias === 'spb');
        }
      }
    });

    this.router.events.subscribe(val => {
     
      /* the router will fire multiple events */
    /* we only want to react if it's the final active route */
      let cityAlias = '';
      if (val instanceof NavigationEnd) {
        this.currentUrl = this.router.url;
        let splitted = this.currentUrl.split("/");
        this.cityAlias = splitted.length > 0 ? splitted[1] : null;
        console.log(this.currentUrl, ' => ', cityAlias);
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
      this.router.navigate([url]);
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
