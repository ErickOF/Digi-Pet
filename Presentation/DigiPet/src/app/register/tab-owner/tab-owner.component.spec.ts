import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TabOwnerComponent } from './tab-owner.component';

describe('TabOwnerComponent', () => {
  let component: TabOwnerComponent;
  let fixture: ComponentFixture<TabOwnerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TabOwnerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TabOwnerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
