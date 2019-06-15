import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminInformationComponent } from './admin-information.component';

describe('AdminInformationComponent', () => {
  let component: AdminInformationComponent;
  let fixture: ComponentFixture<AdminInformationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminInformationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminInformationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
