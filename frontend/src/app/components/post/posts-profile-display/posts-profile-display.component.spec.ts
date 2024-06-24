import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PostsProfileDisplayComponent } from './posts-profile-display.component';

describe('PostsProfileDisplayComponent', () => {
  let component: PostsProfileDisplayComponent;
  let fixture: ComponentFixture<PostsProfileDisplayComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PostsProfileDisplayComponent]
    });
    fixture = TestBed.createComponent(PostsProfileDisplayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
