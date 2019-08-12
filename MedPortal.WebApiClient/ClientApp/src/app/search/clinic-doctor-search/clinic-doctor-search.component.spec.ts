import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClinicDoctorSearchComponent } from './clinic-doctor-search.component';

describe('ClinicDoctorSearchComponent', () => {
  let component: ClinicDoctorSearchComponent;
  let fixture: ComponentFixture<ClinicDoctorSearchComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClinicDoctorSearchComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClinicDoctorSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
