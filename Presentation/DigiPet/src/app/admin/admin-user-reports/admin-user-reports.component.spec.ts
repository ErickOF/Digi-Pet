import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminUserReportsComponent } from './admin-user-reports.component';

describe('AdminUserReportsComponent', () => {
  let component: AdminUserReportsComponent;
  let fixture: ComponentFixture<AdminUserReportsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminUserReportsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminUserReportsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
