import { Component, OnInit } from '@angular/core';
import { ClinicsService } from '../../services/clincs-service';
import { Route, Router, ActivatedRoute, Params } from '@angular/router';
import { IClinic } from '../../data/clinic';
import { Subscription } from 'rxjs';
import { SearchInfoService } from '../../services/search-info-service';

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

  city: string;

  clinics: IClinic[];

  ngOnInit() {
    this.clinicsService.dataReceived.subscribe(clinics => {
      console.log('ClinicListComponent. Clinics received: ', this.clinicsService.clinics);
      this.clinics = this.clinicsService.clinics;
    });

    this.routeParamsSubscription = this.route.params.subscribe(
      (params: Params) => {
        this.city = params['city'];
        console.log('ClinicListComponent. Start getting clinics for ', this.city);
        this.clinicsService.getClinics(this.city);
      });
  }

  ngOnDestroy() {
    if (this.routeParamsSubscription) {
      this.routeParamsSubscription.unsubscribe();
    }
  }

}
