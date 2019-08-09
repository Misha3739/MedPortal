import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { IClinic } from '../../data/clinic';

@Component({
  selector: 'app-clinic-card',
  templateUrl: './clinic-card.component.html',
  styleUrls: ['./clinic-card.component.css']
})
export class ClinicCardComponent implements OnInit {
  @Input() clinic: IClinic;
  @ViewChild('gmap', { static: true }) gmapElement: any;
  map: google.maps.Map;

  isMapShown: boolean = false;
  isMapInitialized: boolean = false;

  constructor() { }

  ngOnInit() {
    
  }

  initMaps(latitude: number, longitude: number) {
    let mapProp = {
      center: new google.maps.LatLng(latitude, longitude),
      zoom: 15,
      mapTypeId: google.maps.MapTypeId.HYBRID
    };

    this.map = new google.maps.Map(this.gmapElement.nativeElement, mapProp);
  }

  showMap() {
    this.isMapShown = !this.isMapShown;
    if (this.isMapShown && !this.isMapInitialized) {
      let latitude = this.clinic.latitude;
      if (latitude == 0) {
        latitude = this.clinic.city.latitude;
      }
      let longitude = this.clinic.latitude;
      if (longitude == 0) {
        longitude = this.clinic.city.longitude;
      }
      this.initMaps(latitude, longitude);
      this.isMapInitialized = true;
    }
  }
}
