import { Component, OnInit, Input } from '@angular/core';
import { IDoctor } from '../../data/doctor';
import { DoctorSlotsService } from '../../services/slots-service';

@Component({
  selector: 'app-doctor-card',
  templateUrl: './doctor-card.component.html',
  styleUrls: ['./doctor-card.component.css']
})
export class DoctorCardComponent implements OnInit {
  @Input() doctor: IDoctor;
  constructor(private slotsService: DoctorSlotsService) { }

  ngOnInit() {
    if (false && this.doctor.originId > 0 && this.doctor.clinics && this.doctor.clinics.length > 0) {
      this.slotsService.getTimeSlots(this.doctor.originId, this.doctor.clinics[0].originId).subscribe((slots) => {
      });
    }
  }

}
