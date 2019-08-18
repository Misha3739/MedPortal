import { Component, OnInit } from '@angular/core';
import { Route, Router, ActivatedRoute, Params } from '@angular/router';
import { Subscription } from 'rxjs';
import { IDoctorDetails } from '../../data/doctor';
import { DoctorsService } from '../../services/doctors-service';

@Component({
  selector: 'app-doctor-details',
  templateUrl: './doctor-details.component.html',
  styleUrls: ['./doctor-details.component.css']
})
export class DoctorDetailsComponent implements OnInit {

  alias: string;
  doctor: IDoctorDetails = {} as IDoctorDetails;

  private routeParamsSubscription: Subscription;
  private doctorDetailsSubscription: Subscription;

  constructor(private doctorService: DoctorsService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit() {
    this.routeParamsSubscription = this.route.params.subscribe(
      (params: Params) => {
        this.alias = params['id'];
        this.doctorService.getDoctorDetails(this.alias);
      });

    this.doctorService.doctorDetailsReceived.subscribe(res => {
      this.doctor = res;
    });
  }

  ngOnDestroy() {
    if (this.routeParamsSubscription) {
      this.routeParamsSubscription.unsubscribe();
    }
    if (this.doctorDetailsSubscription) {
      this.doctorDetailsSubscription.unsubscribe();
    }
  }

}
