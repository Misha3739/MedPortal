import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { SearchMapComponent } from './search/search-map/search-map.component';
import { DoctorListComponent } from './doctors/doctor-list/doctor-list.component';
import { ClinicListComponent } from './clinics/clinic-list/clinic-list.component';
import { DoctorsService } from './services/doctors-service';
import { ClinicsService } from './services/clincs-service';
import { SearchInfoService } from './services/search-info-service';

import { NgSelect2Module } from 'ng-select2';
import { ClinicDetailsComponent } from './clinics/clinic-details/clinic-details.component';
import { DoctorDetailsComponent } from './doctors/doctor-details/doctor-details.component';
import { ClinicCardComponent } from './clinics/clinic-card/clinic-card.component';
import { DoctorCardComponent } from './doctors/doctor-card/doctor-card.component';
import { ClinicSearchComponent } from './clinics/clinic-search/clinic-search.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    SearchMapComponent,
    DoctorListComponent,
    DoctorCardComponent,
    DoctorDetailsComponent,
    ClinicListComponent,
    ClinicCardComponent,
    ClinicDetailsComponent,
    ClinicSearchComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: SearchMapComponent, pathMatch: 'full' }, 
      { path: 'doctors', component: DoctorListComponent },
      { path: 'clinics', component: ClinicListComponent },
      { path: ':city', component: SearchMapComponent, },
      { path: ':city/doctors', component: DoctorListComponent },
      { path: ':city/clinics', component: ClinicListComponent },
      { path: 'clinic/:id', component: ClinicDetailsComponent },
      { path: 'doctor/:id', component: DoctorDetailsComponent },
    ]),
    NgSelect2Module
  ],
  providers: [
    DoctorsService,
    ClinicsService,
    SearchInfoService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

