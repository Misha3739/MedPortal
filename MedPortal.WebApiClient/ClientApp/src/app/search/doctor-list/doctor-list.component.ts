import { Component, OnInit } from '@angular/core';
import { ClinicsService } from '../../services/clincs-service';
import { Route, Router, ActivatedRoute, Params } from '@angular/router';
import { IDoctor } from '../../data/doctor';
import { Subscription } from 'rxjs';
import { DoctorsService } from '../../services/doctors-service';

@Component({
  selector: 'app-doctor-list',
  templateUrl: './doctor-list.component.html',
  styleUrls: ['./doctor-list.component.css']
})
export class DoctorListComponent implements OnInit {

  private city: string;
  private doctors: IDoctor[];

  private routeParamsSubscription: Subscription;

  constructor(private doctorsService: DoctorsService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit() {
    this.routeParamsSubscription = this.route.params.subscribe(
      (params: Params) => {
        this.city = params['city'];
        this.doctors = this.doctorsService.getDoctors(this.city);
      });
  }

  ngOnDestroy() {
    if (this.routeParamsSubscription) {
      this.routeParamsSubscription.unsubscribe();
    }
  }


}
