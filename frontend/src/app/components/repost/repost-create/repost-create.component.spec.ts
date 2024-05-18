import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RepostCreateComponent } from './repost-create.component';

describe('RepostCreateComponent', () => {
  let component: RepostCreateComponent;
  let fixture: ComponentFixture<RepostCreateComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [RepostCreateComponent]
    });
    fixture = TestBed.createComponent(RepostCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
