import { Component, OnInit } from '@angular/core';
import { ClinicsService } from '../../services/clincs-service';
import { Route, Router, ActivatedRoute, Params } from '@angular/router';
import { IClinic } from '../../data/clinic';
import { Subscription } from 'rxjs';
import { SearchInfoService } from '../../services/search-info-service';
import { IClinicSearchParams } from '../../data/clinic-search-params';
import { LocationType } from '../../data/location/location-type';
import { GeolocationService } from '../../services/geolocation-service';

@Component({
  selector: 'app-clinic-list',
  templateUrl: './clinic-list.component.html',
  styleUrls: ['./clinic-list.component.css']
})
export class ClinicListComponent implements OnInit {

  constructor(
    private clinicsService: ClinicsService,
    private geoService: GeolocationService,
    private route: ActivatedRoute) { }


  private routeParamsSubscription: Subscription;
  private queryParamsSubscription: Subscription;

  private searchParams: IClinicSearchParams = {
    city: '',
    speciality: '',
    location: {
      type: LocationType.none,
      alias: ''
    },
    inRange: {
      value: 0
    }
  };

  clinics: IClinic[];

  ngOnInit() {
    this.initPosition();

    this.clinicsService.dataReceived.subscribe(data => {
      console.log('ClinicListComponent. Clinics received ');
      this.clinics = this.clinicsService.clinics;
    });

    this.routeParamsSubscription = this.route.params.subscribe(
      (params: Params) => {
        this.searchParams.city = params['city'];
        console.log('ClinicListComponent. Start getting clinics for ', this.searchParams);
        this.clinicsService.getClinics(this.searchParams);
      });

    this.queryParamsSubscription = this.route.queryParamMap.subscribe(params => {
      this.searchParams.speciality = params.get('speciality');
      this.searchParams.location.type = +params.get('locationType');
      this.searchParams.location.alias = params.get('location');
      this.searchParams.inRange.value = +params.get('inrangekm');
      console.log('ClinicListComponent. Search params: ', this.searchParams);
      this.clinicsService.getClinics(this.searchParams);
    });

    this.geoService.currentPositionChanged.subscribe(pos => {
      this.initPosition();
      if (this.searchParams.inRange.value > 0) {
        this.clinicsService.getClinics(this.searchParams);
      }
    });
  }


  private initPosition() {
      this.searchParams.inRange.coordinates = this.geoService.currentPosition;;
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
