import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClinicSearchComponent } from './clinic-search.component';

describe('ClinicSearchComponent', () => {
  let component: ClinicSearchComponent;
  let fixture: ComponentFixture<ClinicSearchComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClinicSearchComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClinicSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
