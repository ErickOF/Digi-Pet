import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PetCareServicesComponent } from './pet-care-services.component';

describe('PetCareServicesComponent', () => {
  let component: PetCareServicesComponent;
  let fixture: ComponentFixture<PetCareServicesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PetCareServicesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PetCareServicesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
