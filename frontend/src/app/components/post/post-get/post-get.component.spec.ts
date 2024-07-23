import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PostGetComponent } from './post-get.component';

describe('PostGetComponent', () => {
  let component: PostGetComponent;
  let fixture: ComponentFixture<PostGetComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PostGetComponent]
    });
    fixture = TestBed.createComponent(PostGetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
