import { Component, OnInit, ViewChild, Directive, ViewEncapsulation, AfterViewInit } from '@angular/core';
import { } from 'googlemaps';
import { SearchInfoService } from '../../services/search-info-service';
import { Subscription, Subject } from 'rxjs';
import { ActivatedRoute, Router, Params, NavigationEnd } from '@angular/router';
import { ISearchCategory } from '../../data/search-info';
import { SearchInfoType } from '../../data/search-info-type';
import { Select2OptionData } from 'ng-select2';
import { ISearchItem } from '../../data/search-item';
import * as $ from 'jquery';
import { IQuerySearchParams } from '../../data/query-search-params';

@Component({
  selector: 'app-search-map',
  templateUrl: './search-map.component.html',
  styleUrls: ['./search-map.component.css']
})
export class SearchMapComponent implements OnInit, AfterViewInit {
  @ViewChild('gmap', { static: true }) gmapElement: any;
  map: google.maps.Map;

  categories: ISearchCategory[];
  displayCategories: Array<Select2OptionData>;
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
          this.displayCategories = this.toDisplayData(this.clinicsSearchSelected, this.doctorsSearchSelected);
          this.initSelectEvents();
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
        //this.categories = this.searchInfoService.searchInfoItems;
        //this.displayCategories = this.toDisplayData(this.clinicsSearchSelected, this.doctorsSearchSelected);
      } else if (res === 'cities') {
        this.onCityChanged();
      }
      this.initSelectEvents();
    });
  }

  ngAfterViewInit() {
    this.initSelectEvents();
  }

  initSelectEvents() {
    let self = this;
    
    $(document).ready(function () {
      $('.select2-selection__arrow').on('click', function () {
        $('.select2-search__field').on('keyup', function (control) {
          console.log('Select.Jquery. keyup: ', control.key);
          if (control.key) {
            self.suggestion = $('.select2-search__field').val();
            if (self.suggestion.length >= 2) {
              let params = { city: self.city, query: self.suggestion};
              self.searchTerm.next(params);
            }
          }
        });
      });
      
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

  toDisplayData(includeClinics: boolean, includeDoctors: boolean): Select2OptionData[] {
    let result: Array<Select2OptionData> = [];
    for (let category of this.categories) {
      if (!includeClinics && (category.type == SearchInfoType.clinic || category.type == SearchInfoType.clinicSpeciality)) {
        continue;
      }
      if (!includeDoctors && (category.type == SearchInfoType.doctor || category.type == SearchInfoType.doctorSpeciality)) {
        continue;
      }
      let text = this.displayCategory(category.type);
      let item = {
        id: category.type.toString(),
        text: text,
        children: []
      };

      for (let categoryItem of category.items) {
        item.children.push({
          id: category.type.toString() + '@' + categoryItem.alias,
          text: categoryItem.name
        });
      }

      result.push(item);
    }

    return result;
  }

  private getClinic(alias: string): ISearchItem {
    return this.categories.find(c => c.type === SearchInfoType.clinic).items.find(c => c.alias === this.navigateToAlias);
  }

  private getDoctor(alias: string): ISearchItem  {
    return this.categories.find(c => c.type === SearchInfoType.doctor).items.find(c => c.alias === this.navigateToAlias);
  }

  onSelectedValueChanged(event) {
    console.log(event);
    let value = event.value;
    if (value) {
      let splitted = value.split('@', 2);
      this.navigateToResource = parseInt(splitted[0]);
      this.navigateToAlias = splitted[1];
      this.singleItemSelected = this.navigateToResource === SearchInfoType.clinic || this.navigateToResource === SearchInfoType.doctor;
      console.log('Will navigate to ' + SearchInfoType[this.navigateToResource] + ' alias: ' + this.navigateToAlias);

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

  onSelectKeydownEvent(event) {
    console.log('Select. Keydown: ', event);
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
    this.displayCategories = this.toDisplayData(this.clinicsSearchSelected, this.doctorsSearchSelected);
  }

  onClinicCheckChanged() {
    this.displayCategories = this.toDisplayData(this.clinicsSearchSelected, this.doctorsSearchSelected);
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

