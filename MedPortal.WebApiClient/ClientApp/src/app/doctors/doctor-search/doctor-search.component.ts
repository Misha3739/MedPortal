import { Component, OnInit } from '@angular/core';
import { SearchInfoService } from '../../services/search-info-service';
import { ClinicsService } from '../../services/clincs-service';
import { ISpeciality } from '../../data/speciality';
import { ICity } from '../../data/ICity';
import { Subscription } from 'rxjs';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { IClinic } from '../../data/clinic';
import { IClinicSearchParams } from '../../data/clinic-search-params';
import { DoctorsService } from '../../services/doctors-service';

@Component({
  selector: 'app-doctor-search',
  templateUrl: './doctor-search.component.html',
  styleUrls: ['./doctor-search.component.css']
})
export class DoctorSearchComponent implements OnInit {

  constructor(
    private searchInfoService: SearchInfoService,
    private route: ActivatedRoute,
    private router: Router) { }

  private routeParamsSubscription: Subscription;
  private queryParamsSubscription: Subscription;

  private searchParams: IClinicSearchParams = { city: '', speciality: 'noSpeciality' };

  specialities: ISpeciality[] = [];

  nullSpeciality: ISpeciality = { id: 0, alias: 'noSpeciality', name: 'Не выбрано' };

  selectedSpeciality: ISpeciality;

  ngOnInit() {
    this.specialities.push(this.nullSpeciality);
    this.selectedSpeciality = this.nullSpeciality;

    this.routeParamsSubscription = this.route.params.subscribe(
      (params: Params) => {
        this.searchParams.city = params['city'];
      });

    this.queryParamsSubscription = this.route.queryParamMap.subscribe(params => {
      this.searchParams.speciality = params.get('speciality');
      this.setCurrentSpeciality();
    });

    this.searchInfoService.dataReceived.subscribe(event => {
      if (event === 'doctorSpecialities') {
        this.specialities = this.searchInfoService.doctorSpecialities;
        this.specialities.unshift(this.nullSpeciality);
        this.setCurrentSpeciality();
      }
    });

    this.searchInfoService.getDoctorSpecialities();
  }

  setCurrentSpeciality() {
    if (this.searchParams && this.searchParams.speciality && this.searchParams.speciality !== '' && this.specialities) {
      this.selectedSpeciality = this.specialities.find(s => s.alias === this.searchParams.speciality);
    }
  }

  onSpecialityChanged() {
    console.log('DoctorSearchComponent. Speciality changed to: ', this.selectedSpeciality.alias);
    this.searchParams.speciality = this.selectedSpeciality !== this.nullSpeciality ? this.selectedSpeciality.alias : undefined;

    let url = '';
    if (this.searchParams.city && this.searchParams.city !== '') {
      url += this.searchParams.city + '/';
    }
    url += 'doctors';

    this.router.navigate([url], {
      queryParams: {
        speciality: this.searchParams.speciality
      }, queryParamsHandling: null
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