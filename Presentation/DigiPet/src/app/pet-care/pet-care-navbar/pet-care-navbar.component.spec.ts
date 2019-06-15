import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PetCareNavbarComponent } from './pet-care-navbar.component';

describe('PetCareNavbarComponent', () => {
  let component: PetCareNavbarComponent;
  let fixture: ComponentFixture<PetCareNavbarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PetCareNavbarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PetCareNavbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
