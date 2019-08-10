import { Component, OnInit } from '@angular/core';
import { SearchInfoService } from '../../services/search-info-service';
import { ISpeciality } from '../../data/speciality';
import { Subscription } from 'rxjs';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { IClinicSearchParams } from '../../data/clinic-search-params';
import { ILocationCategory } from '../../data/location/location-category';
import { ILocationObject } from '../../data/location/location-object';
import { LocationType } from '../../data/location/location-type';

@Component({
  selector: 'app-clinic-search',
  templateUrl: './clinic-search.component.html',
  styleUrls: ['./clinic-search.component.css']
})
export class ClinicSearchComponent implements OnInit {

  constructor(
    private searchInfoService: SearchInfoService,
    private route: ActivatedRoute,
    private router: Router) { }

  private routeParamsSubscription: Subscription;
  private queryParamsSubscription: Subscription;

  private searchParams: IClinicSearchParams = {
    city: '', speciality: 'noSpeciality', location: {
      type: LocationType.none,
      alias : ''
    }
  };

  specialities: ISpeciality[] = [];
  locationCategories: ILocationCategory[];

  nullSpeciality: ISpeciality = { id: 0, alias: 'noSpeciality', name: 'Не выбрано' };
  nullLocation: ILocationCategory = {
    type: LocationType.none, items: [
      { id: 0, alias: 'noLocation', name: 'Не выбрано', type: LocationType.none }
    ]
  };

  selectedSpeciality: ISpeciality;
  selectedLocation: ILocationObject;

  ngOnInit() {
    this.specialities.push(this.nullSpeciality);
    this.selectedSpeciality = this.nullSpeciality;
    

    this.routeParamsSubscription = this.route.params.subscribe(
      (params: Params) => {
        this.searchParams.city = params['city'];
        this.selectedLocation = this.nullLocation.items[0];
        this.searchInfoService.getLocations(this.searchParams.city);
      });

    this.queryParamsSubscription = this.route.queryParamMap.subscribe(params => {
      this.searchParams.speciality = params.get('speciality');
      this.searchParams.location.type = +params.get('locationType') || LocationType.none;
      this.searchParams.location.alias = params.get('location') || '';
      this.setCurrentSpeciality();
      this.setCurrentLocation();
    });

    this.searchInfoService.dataReceived.subscribe(event => {
      switch (event) {
        case 'clinicSpecialities':
          this.specialities = this.searchInfoService.clinicSpecialities;
          this.specialities.unshift(this.nullSpeciality);
          this.setCurrentSpeciality();
          break;
        case 'locations':
          this.locationCategories = this.searchInfoService.locationCategories;
          this.locationCategories.unshift(this.nullLocation);
          this.setCurrentLocation();
          break;
      } 
    });

    this.searchInfoService.getClinicSpecialities();
    
  }

  setCurrentSpeciality() {
    if (this.searchParams && this.searchParams.speciality && this.searchParams.speciality !== '' && this.specialities) {
      this.selectedSpeciality = this.specialities.find(s => s.alias === this.searchParams.speciality);
    }
  }

  setCurrentLocation() {
    if (this.searchParams && this.searchParams.location && this.searchParams.location.type !== LocationType.none && this.locationCategories) {
      this.selectedLocation = this.locationCategories.find(l => l.type === this.searchParams.location.type).items.find(l => l.alias == this.searchParams.location.alias);
    }
  }

  onSpecialityChanged() {
    console.log('ClinicSearchComponent. Speciality changed to: ', this.selectedSpeciality.alias);
    this.searchParams.speciality = this.selectedSpeciality !== this.nullSpeciality ? this.selectedSpeciality.alias : undefined;
    this.navigate();
  }

  onLocationChanged(event) {
    console.log('ClinicSearchComponent. Location changed to: ', this.selectedLocation);
    this.searchParams.location.type = this.selectedLocation.type;
    this.searchParams.location.alias = this.selectedLocation.alias;
    this.navigate();
  }

  navigate() {
    let url = '';
    if (this.searchParams.city && this.searchParams.city !== '') {
      url += this.searchParams.city + '/';
    }
    url += 'clinics';

    let queryParams: any = {};
    if (this.searchParams.speciality) {
      queryParams.speciality = this.searchParams.speciality;
    }
    if (this.searchParams.location && this.searchParams.location.type != LocationType.none) {
      queryParams.locationType = this.searchParams.location.type;
      queryParams.location = this.searchParams.location.alias;
    }

    this.router.navigate([url], {
      queryParams: queryParams, queryParamsHandling: null
    });
  }

  displayCategory(type: LocationType): string {
    switch (type) {
      case LocationType.none:
        return '';
      case LocationType.street:
        return 'Улицы';
      case LocationType.metroStation:
        return 'Станции метро';
      case LocationType.district:
        return 'Районы';
    }
  }

  ngOnDestroy() {
    if (this.routeParamsSubscription) {
      this.routeParamsSubscription.unsubscribe();
    }
    if (this.queryParamsSubscription) {
      this.queryParamsSubscription.unsubscribe();
    }
  }

}
