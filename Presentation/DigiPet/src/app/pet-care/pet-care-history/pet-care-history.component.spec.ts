import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PetCareHistoryComponent } from './pet-care-history.component';

describe('PetCareHistoryComponent', () => {
  let component: PetCareHistoryComponent;
  let fixture: ComponentFixture<PetCareHistoryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PetCareHistoryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PetCareHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
