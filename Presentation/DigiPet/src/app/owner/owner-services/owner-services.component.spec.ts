import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OwnerServicesComponent } from './owner-services.component';

describe('OwnerServicesComponent', () => {
  let component: OwnerServicesComponent;
  let fixture: ComponentFixture<OwnerServicesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OwnerServicesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OwnerServicesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
