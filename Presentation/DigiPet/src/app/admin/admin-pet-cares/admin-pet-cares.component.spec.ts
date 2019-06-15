import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminPetCaresComponent } from './admin-pet-cares.component';

describe('AdminPetCaresComponent', () => {
  let component: AdminPetCaresComponent;
  let fixture: ComponentFixture<AdminPetCaresComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminPetCaresComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminPetCaresComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
