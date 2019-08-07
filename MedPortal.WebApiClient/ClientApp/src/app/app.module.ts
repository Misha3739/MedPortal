import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HttpClient } from '@angular/common/http';
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
import { GeolocationService } from './services/geolocation-service';
import { DoctorSearchComponent } from './doctors/doctor-search/doctor-search.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    SearchMapComponent,
    DoctorListComponent,
    DoctorCardComponent,
    DoctorDetailsComponent,
    DoctorSearchComponent,
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
      { path: 'doctors', component: DoctorSearchComponent },
      { path: 'clinics', component: ClinicSearchComponent },
      { path: ':city', component: SearchMapComponent, },
      { path: ':city/doctors', component: DoctorSearchComponent },
      { path: ':city/clinics', component: ClinicSearchComponent },
      { path: 'clinics/:id', component: ClinicDetailsComponent },
      { path: 'doctors/:id', component: DoctorDetailsComponent },
      { path: ':city/clinics/:id', component: ClinicDetailsComponent },
      { path: ':city/doctors/:id', component: DoctorDetailsComponent },
    ]),
    NgSelect2Module
  ],
  providers: [
    HttpClient,
    DoctorsService,
    ClinicsService,
    SearchInfoService,
    GeolocationService,
    { provide: 'BASE_URL', useValue: 'http://localhost:5008' }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

