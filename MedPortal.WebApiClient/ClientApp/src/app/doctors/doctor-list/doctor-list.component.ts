import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { Route, Router, ActivatedRoute, Params } from '@angular/router';
import { IDoctor } from '../../data/doctor';
import { Subscription } from 'rxjs';
import { DoctorsService } from '../../services/doctors-service';
import { SearchInfoService } from '../../services/search-info-service';
import { Pagination } from '../../data/Pagination';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { LocationType } from '../../data/location/location-type';
import { UrlQueryParameters } from '../../data/constants/url-query-parameters';
import { ISearchParams } from '../../data/search-params';
import { GeolocationService } from '../../services/geolocation-service';

@Component({
  selector: 'app-doctor-list',
  templateUrl: './doctor-list.component.html',
  styleUrls: ['./doctor-list.component.css']
})
export class DoctorListComponent implements OnInit, AfterViewInit {
  @ViewChild('paginator', { static: true }) paginator: any;
  private dataSource = new MatTableDataSource(Array<IDoctor>());

  constructor(
    private doctorsService: DoctorsService,
    private geoService: GeolocationService,
    private route: ActivatedRoute,
    private router: Router) { }


  private routeParamsSubscription: Subscription;
  private queryParamsSubscription: Subscription;

  private searchParams: ISearchParams = {
    city: '',
    speciality: '',
    location: {
      type: LocationType.none,
      alias: ''
    },
    inRange: {
      value: 0
    }
  };

  doctors: IDoctor[];

  pagination: Pagination = new Pagination();

  firstLoadingCompleted: boolean = false;

  ngOnInit() {
    this.initPosition();
    this.doctorsService.dataReceived.subscribe(data => {
      console.log('DoctorListComponent. Doctors received ');
      this.dataSource.data = this.doctorsService.doctors;
      this.paginate(0, this.pagination.pageSize);
      this.firstLoadingCompleted = true;
    });

    this.routeParamsSubscription = this.route.params.subscribe(
      (params: Params) => {
        this.searchParams.city = params['city'];
        console.log('DoctorListComponent. Getting path parameters... ', this.searchParams);
        //We need to detect only city path changed but not the first loading
        if (this.firstLoadingCompleted) {
          this.doctorsService.getDoctors(this.searchParams);
        }
      });

    this.queryParamsSubscription = this.route.queryParamMap.subscribe(params => {
      this.searchParams.speciality = params.get(UrlQueryParameters.SPECIALITY);
      this.searchParams.location.type = +params.get(UrlQueryParameters.LOCATIONTYPE) || LocationType.none;
      this.searchParams.location.alias = params.get(UrlQueryParameters.LOCATION) || '';
      this.searchParams.inRange.value = +params.get(UrlQueryParameters.INRANGE);
      console.log('DoctorListComponent. Getting query parameters... ', this.searchParams);
      this.doctorsService.getDoctors(this.searchParams);
    });

    this.geoService.currentPositionChanged.subscribe(pos => {
      this.initPosition();
      if (this.searchParams.inRange.value > 0) {
        this.doctorsService.getDoctors(this.searchParams);
      }
    });
  }

  private initPosition() {
    this.searchParams.inRange.coordinates = this.geoService.currentPosition;;
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }

  onPaged(event) {
    console.log('DoctorListComponent. Page changed to: ', event);
    this.paginate(event.pageIndex, event.pageSize);
  }

  private paginate(pageIndex: number, pageSize: number) {
    this.pagination.pageIndex = pageIndex;
    this.pagination.pageSize = pageSize;

    this.doctors = this.dataSource.data.slice(this.pagination.pageIndex * this.pagination.pageSize, (this.pagination.pageIndex + 1) * this.pagination.pageSize);
  }

  ngOnDestroy() {
    if (this.routeParamsSubscription) {
      this.routeParamsSubscription.unsubscribe();
    }
    if (this.queryParamsSubscription) {
      this.queryParamsSubscription.unsubscribe();
    }
  }
}
