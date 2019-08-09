import { Component, OnInit, ViewChild, Directive, ViewEncapsulation, AfterViewInit } from '@angular/core';
import { } from 'googlemaps';
import { SearchInfoService } from '../../services/search-info-service';
import { Subscription, Subject } from 'rxjs';
import { ActivatedRoute, Router, Params, NavigationEnd } from '@angular/router';
import { ISearchCategory } from '../../data/search-info';
import { SearchInfoType } from '../../data/search-info-type';
import { Select2OptionData } from 'ng-select2';
import { ISearchItem } from '../../data/search-item';
import { IQuerySearchParams } from '../../data/query-search-params';
import { MatAutocompleteModule } from '@angular/material/autocomplete';

@Component({
  selector: 'app-search-map',
  templateUrl: './search-map.component.html',
  styleUrls: ['./search-map.component.css']
})
export class SearchMapComponent implements OnInit {
  @ViewChild('gmap', { static: true }) gmapElement: any;
  map: google.maps.Map;

  categories: ISearchCategory[];
  routeParamsSubscription: Subscription;
  city: string;
  singleItemSelected = false;

  navigateToResource: SearchInfoType;
  navigateToAlias: string;

  clinicsSearchSelected = true;
  doctorsSearchSelected = true;

  currentUrl: string;

  suggestion: string = '';
  searchTerm: Subject<IQuerySearchParams>;

  constructor(
    private searchInfoService: SearchInfoService,
    private route: ActivatedRoute,
    private router: Router) {
    this.searchTerm = new Subject<IQuerySearchParams>();
      this.searchInfoService.search(this.searchTerm)
        .subscribe((results: ISearchCategory[]) => {
          console.log('SearchMapComponent. Completed search result:', results);
          this.categories = results;
        });
  }

  ngOnInit() {
    this.initMaps(0, 0);
    this.routeParamsSubscription = this.route.params.subscribe(
      (params: Params) => {
        this.currentUrl = this.router.url;
        this.city = params['city'];
        this.onCityChanged();
        this.searchInfoService.getSearchInfo(this.city);
      });

    this.searchInfoService.dataReceived.subscribe(res => {
      if (res === 'searchItems') {
        this.categories = this.searchInfoService.searchInfoItems;
      } else if (res === 'cities') {
        this.onCityChanged();
      }
    });
  }


  private onCityChanged() {
    console.log('SearchMapComponent. Url changed to : ', this.router.url);
    if (this.city && this.searchInfoService.cities) {
      let cityObject = this.searchInfoService.cities.find(c => c.alias === this.city);
      this.initMaps(cityObject.latitude, cityObject.longitude);
    }
    this.clearSelectedValue();
  }

  private clearSelectedValue() {
    this.navigateToAlias = undefined;
    this.navigateToResource = undefined;
    this.singleItemSelected = false;
  }

  initMaps(latitude: number, longitude: number) {
    let mapProp = {
      center: new google.maps.LatLng(latitude, longitude),
      zoom: 15,
      mapTypeId: google.maps.MapTypeId.HYBRID
    };

    this.map = new google.maps.Map(this.gmapElement.nativeElement, mapProp);
  }

  private getClinic(alias: string): ISearchItem {
    return this.categories.find(c => c.type === SearchInfoType.clinic).items.find(c => c.alias === this.navigateToAlias);
  }

  private getDoctor(alias: string): ISearchItem  {
    return this.categories.find(c => c.type === SearchInfoType.doctor).items.find(c => c.alias === this.navigateToAlias);
  }

  onSelectedValueChanged(value: string) {
    console.log('SearchMapComponent. Selected value changed to : ', value);
    if (value) {
      let splitted = value.split('@', 2);
      this.navigateToResource = parseInt(splitted[0]);
      this.navigateToAlias = splitted[1];
      this.singleItemSelected = this.navigateToResource === SearchInfoType.clinic || this.navigateToResource === SearchInfoType.doctor;
      console.log('Will navigate to ' + SearchInfoType[this.navigateToResource] + ' alias: ' + this.navigateToAlias);

      this.suggestion = this.categories.find(c => c.type === this.navigateToResource).items.find(c => c.alias === this.navigateToAlias).name;

      if (this.navigateToResource === SearchInfoType.clinic) {
        const clinic = this.getClinic(this.navigateToAlias);
        if (clinic.latitude && clinic.longitude) {
          console.log("Latitude: " + clinic.latitude + " longitude: " + clinic.longitude);
          this.initMaps(clinic.latitude, clinic.longitude);
        } else {
          if (clinic.city) {
            this.initMaps(clinic.city.latitude, clinic.city.longitude);
          }
        }
      }
    } else {
       this.clearSelectedValue();
    }
  }

  onSearchInputValueChanged() {
    console.log('SearchMapComponent. Search value changed to : ', this.suggestion);
    if (this.suggestion.length >= 2) {
      let params = { city: this.city, query: this.suggestion };
      this.searchTerm.next(params);
    }
  }


  onRedirect() {
    if (this.singleItemSelected) {
      switch (this.navigateToResource) {
        case SearchInfoType.clinic:
          const clinic = this.getClinic(this.navigateToAlias);
          this.router.navigate([clinic.city.alias + '/clinics/' + this.navigateToAlias]);
          break;
        case SearchInfoType.doctor:
          const doctor = this.getDoctor(this.navigateToAlias);
          this.router.navigate([doctor.city.alias  + '/doctors/' + this.navigateToAlias]);
          break;
      }
    } else {
      let queryParams = {
        speciality: this.navigateToAlias
      };

      if (this.navigateToResource === SearchInfoType.clinicSpeciality) {
        this.router.navigate([this.currentUrl + '/clinics'], { queryParams: queryParams, queryParamsHandling: null });
      } else if (this.navigateToResource === SearchInfoType.doctorSpeciality) {
        this.router.navigate([this.currentUrl + '/doctors'], { queryParams: queryParams, queryParamsHandling: null });
      } else if (this.clinicsSearchSelected) {
        this.router.navigate([this.currentUrl + '/clinics']);
      } else if (this.doctorsSearchSelected) {
        this.router.navigate([this.currentUrl + '/doctors']);
      }
    }
  }

  onDoctorCheckChanged() {
  }

  onClinicCheckChanged() {
  }

  makeOptionUniqueValue(category: ISearchCategory, item: ISearchItem) {
    return category.type.toString() + '@' + item.alias;
  }

  displayCategory(type: SearchInfoType) : string {
    switch (type) {
      case SearchInfoType.clinic:
        return 'Клиники';
      case SearchInfoType.doctor:
        return 'Врачи';
      case SearchInfoType.doctorSpeciality:
        return 'Специальности';
      case SearchInfoType.clinicSpeciality:
        return 'Специализации';
    }
  }

  ngOnDestroy() {
    if (this.routeParamsSubscription) {
      this.routeParamsSubscription.unsubscribe();
    }
  }
}

