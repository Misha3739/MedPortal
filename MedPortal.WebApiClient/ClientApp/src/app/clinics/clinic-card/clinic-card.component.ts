import { Component, OnInit, Input } from '@angular/core';
import { IClinic } from '../../data/clinic';

@Component({
  selector: 'app-clinic-card',
  templateUrl: './clinic-card.component.html',
  styleUrls: ['./clinic-card.component.css']
})
export class ClinicCardComponent implements OnInit {
  @Input() clinic: IClinic;

  constructor() { }

  ngOnInit() {
  }

}
