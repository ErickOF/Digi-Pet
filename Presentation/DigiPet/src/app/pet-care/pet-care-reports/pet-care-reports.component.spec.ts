import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PetCareReportsComponent } from './pet-care-reports.component';

describe('PetCareReportsComponent', () => {
  let component: PetCareReportsComponent;
  let fixture: ComponentFixture<PetCareReportsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PetCareReportsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PetCareReportsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
