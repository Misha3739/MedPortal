import { Component, OnInit, Input } from '@angular/core';
import { IDoctor } from '../../data/doctor';

@Component({
  selector: 'app-doctor-card',
  templateUrl: './doctor-card.component.html',
  styleUrls: ['./doctor-card.component.css']
})
export class DoctorCardComponent implements OnInit {
  @Input() doctor: IDoctor;
  constructor() { }

  ngOnInit() {
  }

}
