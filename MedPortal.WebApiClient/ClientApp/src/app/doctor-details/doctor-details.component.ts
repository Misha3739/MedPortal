import { Component, OnInit } from '@angular/core';
import { Route, Router, ActivatedRoute, Params } from '@angular/router';
import { IClinic } from '../data/clinic';
import { Subscription } from 'rxjs';
import { IDoctorDetails } from '../data/doctor';
import { DoctorsService } from '../services/doctors-service';

@Component({
  selector: 'app-doctor-details',
  templateUrl: './doctor-details.component.html',
  styleUrls: ['./doctor-details.component.css']
})
export class DoctorDetailsComponent implements OnInit {

  id: number;
  doctor: IDoctorDetails;

  private routeParamsSubscription: Subscription;

  constructor(private doctorService: DoctorsService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit() {
    this.routeParamsSubscription = this.route.params.subscribe(
      (params: Params) => {
        this.id = params['id'];
        this.doctor = this.doctorService.getDoctor(this.id);
      });
  }

  ngOnDestroy() {
    if (this.routeParamsSubscription) {
      this.routeParamsSubscription.unsubscribe();
    }
  }

}
