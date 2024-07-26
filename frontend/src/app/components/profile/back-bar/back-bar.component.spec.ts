import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BackBarComponent } from './back-bar.component';

describe('BackBarComponent', () => {
  let component: BackBarComponent;
  let fixture: ComponentFixture<BackBarComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [BackBarComponent]
    });
    fixture = TestBed.createComponent(BackBarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
