import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegisterNavbarComponent } from './register-navbar.component';

describe('RegisterNavbarComponent', () => {
  let component: RegisterNavbarComponent;
  let fixture: ComponentFixture<RegisterNavbarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegisterNavbarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegisterNavbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
