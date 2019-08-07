import { Component, OnInit } from '@angular/core';
import { Route, Router, ActivatedRoute, Params } from '@angular/router';
import { IDoctor } from '../../data/doctor';
import { Subscription } from 'rxjs';
import { DoctorsService } from '../../services/doctors-service';
import { IDoctorSearchParams } from '../../data/doctor-search-params';
import { SearchInfoService } from '../../services/search-info-service';

@Component({
  selector: 'app-doctor-list',
  templateUrl: './doctor-list.component.html',
  styleUrls: ['./doctor-list.component.css']
})
export class DoctorListComponent implements OnInit {

  constructor(
    private searchInfoService: SearchInfoService,
    private doctorsService: DoctorsService,
    private route: ActivatedRoute,
    private router: Router) { }


  private routeParamsSubscription: Subscription;
  private queryParamsSubscription: Subscription;

  private searchParams: IDoctorSearchParams = { city: '', speciality: '' };

  doctors: IDoctor[];

  ngOnInit() {
    this.doctorsService.dataReceived.subscribe(data => {
      console.log('DoctorListComponent. Doctors received ');
      this.doctors = this.doctorsService.doctors;
    });

    this.routeParamsSubscription = this.route.params.subscribe(
      (params: Params) => {
        this.searchParams.city = params['city'];
        console.log('DoctorListComponent. Start getting doctors for ', this.searchParams);
        this.doctorsService.getDoctors(this.searchParams);
      });

    this.queryParamsSubscription = this.route.queryParamMap.subscribe(params => {
      this.searchParams.speciality = params.get('speciality');
      this.doctorsService.getDoctors(this.searchParams);
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
