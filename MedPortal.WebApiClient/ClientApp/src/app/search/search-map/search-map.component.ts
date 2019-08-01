import { Component, OnInit, ViewChild, Directive, ViewEncapsulation } from '@angular/core';
import { } from 'googlemaps';
import { SearchInfoService } from '../../services/search-info-service';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { ISearchCategory } from '../../data/search-info';
import { SearchInfoType } from '../../data/search-info-type';
import { Select2OptionData } from 'ng-select2';


@Component({
  selector: 'app-search-map',
  templateUrl: './search-map.component.html',
  styleUrls: ['./search-map.component.css']
})
export class SearchMapComponent implements OnInit {
  @ViewChild('gmap', { static: true }) gmapElement: any;
  map: google.maps.Map;

  categories: ISearchCategory[];
  displayCategories: Array<Select2OptionData>;
  routeParamsSubscription: Subscription;
  city: string;
  isSelected = false;

  navigateToResource: SearchInfoType;
  navigateToAlias: string;

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
        this.displayCategories = this.toDisplayData();
      });
  }

  toDisplayData() : Select2OptionData[] {
    let result: Array<Select2OptionData> = [];
    for (let category of this.categories) {
      let text = this.displayCategory(category.type);
      let item = {
        id: category.type.toString(),
        text: text,
        children: []
      };

      for (let categoryItem of category.items) {
        item.children.push({
          id: category.type.toString() + '@' + categoryItem.alias,
          text: categoryItem.name
        });
      }

      result.push(item);
    }

    return result;
  }

  onSelectedValueChanged(event) {
    console.log(event);
    let value = event.value;
    if (value) {
      let splitted = value.split('@', 2);
      this.navigateToResource = parseInt(splitted[0]);
      this.navigateToAlias = splitted[1];
      this.isSelected = true;
      console.log('Will navigate to ' + SearchInfoType[this.navigateToResource] + ' alias: ' + this.navigateToAlias);
    } else {
      console.error('Selector value is not defined');
    }
  }

  onRedirect() {
    switch (this.navigateToResource) {
      case SearchInfoType.clinic:
        this.router.navigate(['clinic/' + this.navigateToAlias]);
        break;
      case SearchInfoType.doctor:
        this.router.navigate(['doctor/' + this.navigateToAlias]);
        break;
      case SearchInfoType.speciality:
        this.router.navigate(['speciality/' + this.navigateToAlias]);
        break;
    }
   
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

