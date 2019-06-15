import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PetCareInformationComponent } from './pet-care-information.component';

describe('PetCareInformationComponent', () => {
  let component: PetCareInformationComponent;
  let fixture: ComponentFixture<PetCareInformationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PetCareInformationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PetCareInformationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
