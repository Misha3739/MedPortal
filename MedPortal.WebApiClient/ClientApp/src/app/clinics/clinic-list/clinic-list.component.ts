import { Component, OnInit } from '@angular/core';
import { ClinicsService } from '../../services/clincs-service';
import { Route, Router, ActivatedRoute, Params } from '@angular/router';
import { IClinic } from '../../data/clinic';
import { Subscription } from 'rxjs';
import { SearchInfoService } from '../../services/search-info-service';
import { IClinicSearchParams } from '../../data/clinic-search-params';

@Component({
  selector: 'app-clinic-list',
  templateUrl: './clinic-list.component.html',
  styleUrls: ['./clinic-list.component.css']
})
export class ClinicListComponent implements OnInit {

  constructor(
    private searchInfoService: SearchInfoService,
    private clinicsService: ClinicsService,
    private route: ActivatedRoute,
    private router: Router) { }


  private routeParamsSubscription: Subscription;
  private queryParamsSubscription: Subscription;

  private searchParams: IClinicSearchParams = { city: '', speciality: '' };

  clinics: IClinic[];

  ngOnInit() {
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
      this.clinicsService.getClinics(this.searchParams);
    });
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
