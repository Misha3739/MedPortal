import { Component, OnInit } from '@angular/core';
import { SearchInfoService } from '../../services/search-info-service';
import { ISpeciality } from '../../data/speciality';
import { Subscription } from 'rxjs';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { ISearchParams } from '../../data/search-params';
import { ILocationCategory } from '../../data/location/location-category';
import { ILocationObject } from '../../data/location/location-object';
import { LocationType } from '../../data/location/location-type';
import { ClinicsService } from '../../services/clincs-service';
import { IClinic } from '../../data/clinic';
import { GeolocationService } from '../../services/geolocation-service';
import { IRange } from '../../data/location/range';
import { ICoordinates } from '../../data/location/coordinates';
import { UrlQueryParameters } from '../../data/constants/url-query-parameters';
import { IDoctor } from '../../data/doctor';
import { DoctorsService } from '../../services/doctors-service';

@Component({
  selector: 'app-clinic-doctor-search',
  templateUrl: './clinic-doctor-search.component.html',
  styleUrls: ['./clinic-doctor-search.component.css']
})
export class ClinicDoctorSearchComponent implements OnInit {
  isClinicComponent = true;

  constructor(
    private searchInfoService: SearchInfoService,
    private geoService: GeolocationService,
    private clinicService: ClinicsService,
    private doctorsService: DoctorsService,
    private route: ActivatedRoute,
    private router: Router) { }

  private routeParamsSubscription: Subscription;
  private queryParamsSubscription: Subscription;
  private searchInfoServiceSubscription: Subscription;
  private geoServiceSubscription: Subscription;
  private dataServiceSubscription: Subscription;

  private searchParams: ISearchParams = {
    city: '', speciality: 'noSpeciality',
    location: {
      type: LocationType.none,
      alias: ''
    }, inRange: {
      value: 0
    }
  };

  clinics: IClinic[];
  doctors: IDoctor[];

  specialities: ISpeciality[] = [];
  locationCategories: ILocationCategory[];
  ranges: IRange[] = [];
  currentRange: IRange;

  nullSpeciality: ISpeciality = { id: 0, alias: 'noSpeciality', name: 'Не выбрано' };
  nullLocation: ILocationCategory = {
    type: LocationType.none, items: [
      { id: 0, alias: 'noLocation', name: 'Не выбрано', type: LocationType.none }
    ]
  };

  selectedSpeciality: ISpeciality;
  selectedLocation: ILocationObject;

  center: ICoordinates;

  ngOnInit() {
    this.initPosition();
    this.specialities.push(this.nullSpeciality);
    this.selectedSpeciality = this.nullSpeciality;


    this.routeParamsSubscription = this.route.params.subscribe(
      (params: Params) => {
        this.searchParams.city = params['city'];
        this.isClinicComponent = this.router.url.indexOf('clinics') !== -1;
        this.initRanges();
        this.selectedLocation = this.nullLocation.items[0];
        this.searchInfoService.getLocations(this.searchParams.city);
      });

    this.queryParamsSubscription = this.route.queryParamMap.subscribe(params => {
      this.searchParams.speciality = params.get(UrlQueryParameters.SPECIALITY);
      this.searchParams.location.type = +params.get(UrlQueryParameters.LOCATIONTYPE) || LocationType.none;
      this.searchParams.location.alias = params.get(UrlQueryParameters.LOCATION) || '';
      this.searchParams.inRange.value = +params.get(UrlQueryParameters.INRANGE);
      this.setCurrentValues();
    });

    if (this.isClinicComponent) {
      this.dataServiceSubscription = this.clinicService.dataReceived.subscribe(event => {
        this.clinics = this.clinicService.clinics;
      });
    } else {
      this.dataServiceSubscription = this.doctorsService.dataReceived.subscribe(event => {
        this.doctors = this.doctorsService.doctors;
      });
    }

    this.searchInfoServiceSubscription = this.searchInfoService.dataReceived.subscribe(event => {
      switch (event) {
        case 'clinicSpecialities':
          this.specialities = this.searchInfoService.clinicSpecialities;
          this.specialities.unshift(this.nullSpeciality);
          this.setCurrentValues();
          break;
        case 'doctorSpecialities':
          this.specialities = this.searchInfoService.doctorSpecialities;
          this.specialities.unshift(this.nullSpeciality);
          this.setCurrentValues();
          break;
        case 'locations':
          this.locationCategories = this.searchInfoService.locationCategories;
          this.locationCategories.unshift(this.nullLocation);
          this.setCurrentValues();
          break;
      }
    });

    this.geoServiceSubscription = this.geoService.currentPositionChanged.subscribe(pos => {
      this.initPosition();
    });

    if (this.isClinicComponent) {
      this.searchInfoService.getClinicSpecialities();
    } else {
      this.searchInfoService.getDoctorSpecialities();
    }
  }

  private initPosition() {
    this.center = this.geoService.currentPosition;
    this.searchParams.inRange.coordinates = this.center;
  }

  private initRanges() {
    let name = this.isClinicComponent ? 'Все клиники' : 'Все доктора';
    this.ranges.push({ name: name, id: 0 });
    for (let i = 1; i < 15; i++) {
      this.ranges.push({ id: i, name: i.toString() + ' km' });
    }
    this.currentRange = this.ranges[0];
  }

  setCurrentValues() {
    if (this.searchParams) {
      if (this.searchParams.speciality && this.searchParams.speciality !== '' && this.specialities) {
        this.selectedSpeciality = this.specialities.find(s => s.alias === this.searchParams.speciality);
      }
      if (this.searchParams.location && this.searchParams.location.type !== LocationType.none && this.locationCategories) {
        this.selectedLocation = this.locationCategories.find(l => l.type === this.searchParams.location.type).items.find(l => l.alias == this.searchParams.location.alias);
      }

      if (this.searchParams.inRange) {
        this.currentRange = this.ranges.find(l => l.id == this.searchParams.inRange.value);
      }
    }
  }

  onSpecialityChanged() {
    console.log('SearchComponent. Speciality changed to: ', this.selectedSpeciality.alias);
    this.searchParams.speciality = this.selectedSpeciality !== this.nullSpeciality ? this.selectedSpeciality.alias : undefined;
    this.navigate();
  }

  onLocationChanged(event) {
    console.log('SearchComponent. Location changed to: ', this.selectedLocation);
    this.searchParams.location.type = this.selectedLocation.type;
    this.searchParams.location.alias = this.selectedLocation.alias;
    this.navigate();
  }

  onRangeChanged(event) {
    console.log('SearchComponent. Range changed to: ', this.currentRange);
    this.searchParams.inRange.value = this.currentRange.id;
    this.navigate();
  }

  navigate() {
    let url = '';
    if (this.searchParams.city && this.searchParams.city !== '') {
      url += this.searchParams.city + '/';
    }
    let urlPart = this.isClinicComponent ? 'clinics' : 'doctors';
    url += urlPart;

    let queryParams: any = {};
    if (this.searchParams.speciality) {
      queryParams[UrlQueryParameters.SPECIALITY] = this.searchParams.speciality;
    }
    if (this.searchParams.location && this.searchParams.location.type != LocationType.none) {
      queryParams[UrlQueryParameters.LOCATIONTYPE] = this.searchParams.location.type;
      queryParams[UrlQueryParameters.LOCATION] = this.searchParams.location.alias;
    }
    if (this.searchParams.inRange.value > 0) {
      queryParams[UrlQueryParameters.INRANGE] = this.searchParams.inRange.value;
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
    if (this.searchInfoServiceSubscription) {
      this.searchInfoServiceSubscription.unsubscribe();
    }
    if (this.geoServiceSubscription) {
      this.geoServiceSubscription.unsubscribe();
    }
    if (this.dataServiceSubscription) {
      this.dataServiceSubscription.unsubscribe();
    }
  }
}
