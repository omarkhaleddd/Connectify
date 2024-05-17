import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RepostGetComponent } from './repost-get.component';

describe('RepostGetComponent', () => {
  let component: RepostGetComponent;
  let fixture: ComponentFixture<RepostGetComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [RepostGetComponent]
    });
    fixture = TestBed.createComponent(RepostGetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
