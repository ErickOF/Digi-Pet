import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PetCareScheduleComponent } from './pet-care-schedule.component';

describe('PetCareScheduleComponent', () => {
  let component: PetCareScheduleComponent;
  let fixture: ComponentFixture<PetCareScheduleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PetCareScheduleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PetCareScheduleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
