import { Component, OnInit, ViewChild, Directive, ViewEncapsulation } from '@angular/core';
import { } from 'googlemaps';
import { SearchInfoService } from '../../services/search-info-service';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Router, Params, NavigationEnd } from '@angular/router';
import { ISearchCategory } from '../../data/search-info';
import { SearchInfoType } from '../../data/search-info-type';
import { Select2OptionData } from 'ng-select2';
import { ClinicsService } from '../../services/clincs-service';


@Component({
  selector: 'app-search-map',
  templateUrl: './search-map.component.html',
  styleUrls: ['./search-map.component.css']
})
export class SearchMapComponent implements OnInit {
  @ViewChild('gmap', { static: true }) gmapElement: any;
  map: google.maps.Map;

  categories: ISearchCategory[];
  displayCategories: Array<Select2OptionData>;
  routeParamsSubscription: Subscription;
  city: string;
  singleItemSelected = false;

  navigateToResource: SearchInfoType;
  navigateToAlias: string;

  clinicsSearchSelected = false;
  doctorsSearchSelected = false;

  currentUrl: string;

  constructor(private searchInfoService: SearchInfoService, private clinicsService: ClinicsService, private route: ActivatedRoute, private router: Router) { }

  ngOnInit() {
    this.initMaps(59.93863, 30.31413);

    this.routeParamsSubscription = this.route.params.subscribe(
      (params: Params) => {
        this.currentUrl = this.router.url;
        this.city = params['city'];
        this.categories = this.searchInfoService.getSearchInfo(this.city);
        this.displayCategories = this.toDisplayData();
      });
  }

  initMaps(latitude: number, longtitude: number) {
    let mapProp = {
      center: new google.maps.LatLng(latitude, longtitude),
      zoom: 15,
      mapTypeId: google.maps.MapTypeId.HYBRID
    };

    this.map = new google.maps.Map(this.gmapElement.nativeElement, mapProp);
  }

  toDisplayData() : Select2OptionData[] {
    let result: Array<Select2OptionData> = [];
    for (let category of this.categories) {
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
        let clinic = this.clinicsService.getClinic(this.navigateToAlias);
        console.log("Latitude: " + clinic.latitude + " longtitude: " + clinic.longtitude);
        this.initMaps(clinic.latitude, clinic.longtitude);
      }

    } else {
      console.error('Selector value is not defined');
    }
  }

  onRedirect() {
    if (this.singleItemSelected) {
      switch (this.navigateToResource) {
        case SearchInfoType.clinic:
          this.router.navigate(['clinics/' + this.navigateToAlias]);
          break;
        case SearchInfoType.doctor:
          this.router.navigate(['doctors/' + this.navigateToAlias]);
          break;
      }
    } else {
      if (this.clinicsSearchSelected && this.doctorsSearchSelected) {

      } else if (this.clinicsSearchSelected && !this.doctorsSearchSelected) {
        this.router.navigate([this.currentUrl + '/clinics']);
      } else if (!this.clinicsSearchSelected && this.doctorsSearchSelected) {
        this.router.navigate([this.currentUrl + '/doctors']);
      }
    }
  }

  displayCategory(type: SearchInfoType) : string {
    switch (type) {
      case SearchInfoType.clinic:
        return 'Клиники';
      case SearchInfoType.doctor:
        return 'Врачи';
      case SearchInfoType.speciality:
        return 'Специальности';
    }
  }

  ngOnDestroy() {
    if (this.routeParamsSubscription) {
      this.routeParamsSubscription.unsubscribe();
    }
  }
}

