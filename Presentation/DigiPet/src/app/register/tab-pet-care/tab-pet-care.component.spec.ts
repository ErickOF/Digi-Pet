import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TabPetCareComponent } from './tab-pet-care.component';

describe('TabPetCareComponent', () => {
  let component: TabPetCareComponent;
  let fixture: ComponentFixture<TabPetCareComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TabPetCareComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TabPetCareComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
