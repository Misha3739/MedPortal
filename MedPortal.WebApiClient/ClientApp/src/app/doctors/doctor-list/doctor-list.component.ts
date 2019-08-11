import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { Route, Router, ActivatedRoute, Params } from '@angular/router';
import { IDoctor } from '../../data/doctor';
import { Subscription } from 'rxjs';
import { DoctorsService } from '../../services/doctors-service';
import { IDoctorSearchParams } from '../../data/doctor-search-params';
import { SearchInfoService } from '../../services/search-info-service';
import { Pagination } from '../../data/Pagination';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';

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
    private route: ActivatedRoute,
    private router: Router) { }


  private routeParamsSubscription: Subscription;
  private queryParamsSubscription: Subscription;

  private searchParams: IDoctorSearchParams = { city: '', speciality: '' };

  doctors: IDoctor[];

  pagination: Pagination = new Pagination();

  ngOnInit() {
    this.doctorsService.dataReceived.subscribe(data => {
      console.log('DoctorListComponent. Doctors received ');
      this.dataSource.data = this.doctorsService.doctors;
      this.paginate(0, this.pagination.pageSize);
    });

    this.routeParamsSubscription = this.route.params.subscribe(
      (params: Params) => {
        this.searchParams.city = params['city'];
        console.log('DoctorListComponent. Start getting doctors for ', this.searchParams);
        this.doctorsService.getDoctors(this.searchParams);
      });

    this.queryParamsSubscription = this.route.queryParamMap.subscribe(params => {
      this.searchParams.speciality = params.get('speciality');
      this.doctorsService.getDoctors(this.searchParams);
    });
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
