import { Component, OnInit } from '@angular/core';
import { ClinicsService } from '../../services/clincs-service';
import { Route, Router, ActivatedRoute, Params } from '@angular/router';
import { IClinic } from '../../data/clinic';
import { Subscription } from 'rxjs';
import { SearchInfoService } from '../../services/search-info-service';
import { ISearchParams } from '../../data/search-params';
import { LocationType } from '../../data/location/location-type';
import { GeolocationService } from '../../services/geolocation-service';
import { UrlQueryParameters } from '../../data/constants/url-query-parameters';

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

  private searchParams: ISearchParams = {
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
  firstLoadingCompleted: boolean = false;

  ngOnInit() {
    this.initPosition();

    this.clinicsService.dataReceived.subscribe(data => {
      console.log('ClinicListComponent. Clinics received ');
      this.clinics = this.clinicsService.clinics;
      this.firstLoadingCompleted = true;
    });

    this.routeParamsSubscription = this.route.params.subscribe(
      (params: Params) => {
        this.searchParams.city = params['city'];
        console.log('ClinicListComponent. Start getting clinics for ', this.searchParams);
        //We need to detect only city path changed but not the first loading
        if (this.firstLoadingCompleted) {
          this.clinicsService.getClinics(this.searchParams);
        }
      });

    this.queryParamsSubscription = this.route.queryParamMap.subscribe(params => {
      this.searchParams.speciality = params.get(UrlQueryParameters.SPECIALITY);
      this.searchParams.location.type = +params.get(UrlQueryParameters.LOCATIONTYPE) || LocationType.none;
      this.searchParams.location.alias = params.get(UrlQueryParameters.LOCATION) || '';
      this.searchParams.inRange.value = +params.get(UrlQueryParameters.INRANGE);
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
