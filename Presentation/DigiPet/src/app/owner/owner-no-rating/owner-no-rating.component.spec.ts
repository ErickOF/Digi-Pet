import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OwnerNoRatingComponent } from './owner-no-rating.component';

describe('OwnerNoRatingComponent', () => {
  let component: OwnerNoRatingComponent;
  let fixture: ComponentFixture<OwnerNoRatingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OwnerNoRatingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OwnerNoRatingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
