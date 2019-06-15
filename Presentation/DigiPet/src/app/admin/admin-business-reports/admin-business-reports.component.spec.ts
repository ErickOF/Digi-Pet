import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminBusinessReportsComponent } from './admin-business-reports.component';

describe('AdminBusinessReportsComponent', () => {
  let component: AdminBusinessReportsComponent;
  let fixture: ComponentFixture<AdminBusinessReportsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminBusinessReportsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminBusinessReportsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
