import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FilterationBarComponent } from './filteration-bar.component';

describe('FilterationBarComponent', () => {
  let component: FilterationBarComponent;
  let fixture: ComponentFixture<FilterationBarComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FilterationBarComponent]
    });
    fixture = TestBed.createComponent(FilterationBarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
