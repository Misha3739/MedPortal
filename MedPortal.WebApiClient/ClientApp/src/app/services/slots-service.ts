import { Injectable, Inject } from "@angular/core";
import { IDoctor } from "../data/doctor";
import { IClinic, IClinicDetails } from "../data/clinic";
import { Subject, Observable } from "rxjs";
import { HttpHeaders, HttpClient, HttpParams } from "@angular/common/http";
import { IClinicSearchParams } from "../data/clinic-search-params";
import { LocationType } from "../data/location/location-type";
import { ITimeSlot, IDoctorClinicTimeSlot } from "../data/slots/doctor-slot";

@Injectable()
export class DoctorSlotsService {

  static instance: DoctorSlotsService;

  private headers = new HttpHeaders({
    'Content-Type': 'application/json',
    'Authorization': 'Basic partner.16844:z790bIpI'
  });

  constructor(@Inject('DOCDOC_BASE_URL') public baseUrl: string, private httpClient: HttpClient) {
    return DoctorSlotsService.instance = DoctorSlotsService.instance || this;
  }

 getTimeSlots(clinicOriginId: number, doctorOriginId: number): Observable<ITimeSlot[]> {
    let from = new Date();
    let to = this.addDays(from, 7);
   let url = this.baseUrl + '/slot/list/doctor/' + doctorOriginId + '/clinic/'+ clinicOriginId +'/from/'+ from +'/to/' + to
    return Observable.create(
      (observer) => {
        this.httpClient.get(url).subscribe((data: ITimeSlot[]) => {
          console.log('DoctorSlotsService. Position received: ', data);
          observer.next(data);
        }),
          () => {
            console.log('Position is not available');
          },
          {
            enableHighAccuracy: true
          };
      });
  }

  private addDays(date: Date, days: number): Date {
    date.setDate(date.getDate() + days);
    return date;
  }
}
