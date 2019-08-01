import { Component, OnInit } from '@angular/core';
import { ClinicsService } from '../../services/clincs-service';
import { Route, Router, ActivatedRoute, Params } from '@angular/router';
import { IClinic } from '../../data/clinic';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-clinic-list',
  templateUrl: './clinic-list.component.html',
  styleUrls: ['./clinic-list.component.css']
})
export class ClinicListComponent implements OnInit {

  private city: string;
  private clinics: IClinic[];

  private routeParamsSubscription: Subscription;

  constructor(private clinicsService: ClinicsService,
    private route: ActivatedRoute,
    private router: Router,) { }

  ngOnInit() {
    this.routeParamsSubscription = this.route.params.subscribe(
      (params: Params) => {
        this.city = params['city'];
        this.clinics = this.clinicsService.getClinics(this.city);
      });
  }

  ngOnDestroy() {
    if (this.routeParamsSubscription) {
      this.routeParamsSubscription.unsubscribe();
    }
  }

}
