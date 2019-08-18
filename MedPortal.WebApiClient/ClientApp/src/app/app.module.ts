import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { APP_BASE_HREF } from '@angular/common';

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
import { GeolocationService } from './services/geolocation-service';

import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';

import { AgmCoreModule } from '@agm/core';
import { DoctorSlotsService } from './services/slots-service';
import { ClinicDoctorSearchComponent } from './search/clinic-doctor-search/clinic-doctor-search.component';
import { CacheService } from './services/cache-service';

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
    ClinicDoctorSearchComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: SearchMapComponent, pathMatch: 'full' },
      { path: ':city', component: SearchMapComponent, },
      { path: 'doctors', component: ClinicDoctorSearchComponent, children: [
          { path: '', component: DoctorListComponent }
      ]},
      { path: ':city/doctors', component: ClinicDoctorSearchComponent, children: [
          { path: '', component: DoctorListComponent }
      ]},
      { path: 'clinics', component: ClinicDoctorSearchComponent, children: [
        { path: '', component: ClinicListComponent }
      ]},
      { path: ':city/clinics', component: ClinicDoctorSearchComponent, children: [
          { path: '', component: ClinicListComponent }
      ]},
      { path: 'clinics/:id', component: ClinicDetailsComponent },
      { path: 'doctors/:id', component: DoctorDetailsComponent },
      { path: ':city/clinics/:id', component: ClinicDetailsComponent },
      { path: ':city/doctors/:id', component: DoctorDetailsComponent },
    ]),
    NgSelect2Module,
    MatAutocompleteModule,
    BrowserAnimationsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatPaginatorModule,
    MatTableModule,
    AgmCoreModule.forRoot({
      // please get your own API key here:
      // https://developers.google.com/maps/documentation/javascript/get-api-key?hl=en
      apiKey: 'AIzaSyCI5n732JUuTrk5uFpnZ14R_h6A0Xsf1XE'
    })
  ],
  providers: [
    HttpClient,
    DoctorsService,
    ClinicsService,
    SearchInfoService,
    GeolocationService,
    DoctorSlotsService,
    CacheService,
    { provide: 'BASE_URL', useValue: 'http://localhost:5008' },
    { provide: 'DOCDOC_BASE_URL', useValue: 'https://api.docdoc.ru/public/rest/1.0.12' },
    { provide: APP_BASE_HREF, useValue: '/' }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

