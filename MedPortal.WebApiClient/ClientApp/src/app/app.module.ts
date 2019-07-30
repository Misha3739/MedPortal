import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { SearchMapComponent } from './search/search-map/search-map.component';
import { DoctorListComponent } from './search/doctor-list/doctor-list.component';
import { ClinicListComponent } from './search/clinic-list/clinic-list.component';
import { DoctorsService } from './services/doctors-service';
import { ClinicsService } from './services/clincs-service';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    SearchMapComponent,
    DoctorListComponent,
    ClinicListComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: SearchMapComponent, pathMatch: 'full' },
      { path: ':city', component: SearchMapComponent, children: [
        { path: 'doctors', component: DoctorListComponent },
        { path: 'clinics', component: ClinicListComponent },
      ]
      }
    ])
  ],
  providers: [
    DoctorsService,
    ClinicsService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

