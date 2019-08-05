import { Component, OnInit } from '@angular/core';
import { SearchInfoService } from '../../services/search-info-service';
import { ClinicsService } from '../../services/clincs-service';
import { ISpeciality } from '../../data/speciality';
import { ICity } from '../../data/ICity';

@Component({
  selector: 'app-clinic-search',
  templateUrl: './clinic-search.component.html',
  styleUrls: ['./clinic-search.component.css']
})
export class ClinicSearchComponent implements OnInit {

  constructor(private searchInfoService: SearchInfoService, private clinicsService: ClinicsService) { }

  specialities: ISpeciality[];

  cities: ICity[];

  ngOnInit() {
    this.specialities = this.searchInfoService.getSpecialities();
    this.cities = this.searchInfoService.getCitiesOld();
  }

}
