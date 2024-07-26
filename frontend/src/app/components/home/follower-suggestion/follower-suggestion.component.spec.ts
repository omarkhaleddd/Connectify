import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FollowerSuggestionComponent } from './follower-suggestion.component';

describe('FollowerSuggestionComponent', () => {
  let component: FollowerSuggestionComponent;
  let fixture: ComponentFixture<FollowerSuggestionComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FollowerSuggestionComponent]
    });
    fixture = TestBed.createComponent(FollowerSuggestionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
