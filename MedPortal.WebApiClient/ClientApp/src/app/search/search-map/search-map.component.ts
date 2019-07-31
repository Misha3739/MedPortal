import { Component, OnInit, ViewChild, Directive, ViewEncapsulation } from '@angular/core';
import { } from 'googlemaps';
import { SearchInfoService } from '../../services/search-info-service';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { ISearchCategory } from '../../data/search-info';
import { SearchInfoType } from '../../data/search-info-type';


@Component({
  selector: 'app-search-map',
  templateUrl: './search-map.component.html',
  styleUrls: ['./search-map.component.css']
})
export class SearchMapComponent implements OnInit {
  @ViewChild('gmap', { static: true }) gmapElement: any;
  map: google.maps.Map;

  categories: ISearchCategory[];
  routeParamsSubscription: Subscription;
  city: string;

  constructor(private searchInfoService: SearchInfoService, private route: ActivatedRoute, private router: Router) { }

  ngOnInit() {
    let mapProp = {
      center: new google.maps.LatLng(18.5793, 73.8143),
      zoom: 15,
      mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    this.map = new google.maps.Map(this.gmapElement.nativeElement, mapProp);

    this.routeParamsSubscription = this.route.params.subscribe(
      (params: Params) => {
        this.city = params['city'];
        this.categories = this.searchInfoService.getSearchInfo(this.city);
      });
  }

  displayCategory(type: SearchInfoType) : string {
    switch (type) {
      case SearchInfoType.clinic:
        return 'Клиники';
      case SearchInfoType.doctor:
        return 'Врачи';
      case SearchInfoType.speciality:
        return 'Специальности';
    }
  }

  ngOnDestroy() {
    if (this.routeParamsSubscription) {
      this.routeParamsSubscription.unsubscribe();
    }
  }
}
