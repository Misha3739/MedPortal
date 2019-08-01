import { Component, OnInit } from '@angular/core';
import { Route, Router, ActivatedRoute, Params } from '@angular/router';
import { Subscription } from 'rxjs';
import { IClinicDetails } from '../../data/clinic';
import { ClinicsService } from '../../services/clincs-service';

@Component({
  selector: 'app-clinic-details',
  templateUrl: './clinic-details.component.html',
  styleUrls: ['./clinic-details.component.css']
})
export class ClinicDetailsComponent implements OnInit {
  private routeParamsSubscription: Subscription;

  id: number;
  clinic: IClinicDetails;

  constructor(private clinicsService: ClinicsService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit() {
    this.routeParamsSubscription = this.route.params.subscribe(
      (params: Params) => {
        this.id = params['id'];
        this.clinic = this.clinicsService.getClinic(this.id);
      });
  }

  ngOnDestroy() {
    if (this.routeParamsSubscription) {
      this.routeParamsSubscription.unsubscribe();
    }
  }

}
