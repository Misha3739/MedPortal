import { Component, OnInit } from '@angular/core';
import { SearchInfoService } from '../../services/search-info-service';
import { ClinicsService } from '../../services/clincs-service';
import { ISpeciality } from '../../data/speciality';
import { ICity } from '../../data/ICity';
import { Subscription } from 'rxjs';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { IClinic } from '../../data/clinic';

@Component({
  selector: 'app-clinic-search',
  templateUrl: './clinic-search.component.html',
  styleUrls: ['./clinic-search.component.css']
})
export class ClinicSearchComponent implements OnInit {

  constructor(
    private searchInfoService: SearchInfoService,
    private clinicsService: ClinicsService,
    private route: ActivatedRoute,
    private router: Router) { }

  private routeParamsSubscription: Subscription;

  city: string;

  specialities: ISpeciality[];

  ngOnInit() {
    this.routeParamsSubscription = this.route.params.subscribe(
      (params: Params) => {
        this.city = params['city'];
        console.log('ClinicSearchComponent. Start getting clinics for ', this.city);
        this.clinicsService.getClinics(this.city );
      });

    this.specialities = this.searchInfoService.getSpecialities();
  }

  ngOnDestroy() {
    if (this.routeParamsSubscription) {
      this.routeParamsSubscription.unsubscribe();
    }
  }

}
