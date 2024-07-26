import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChatSearchComponent } from './chat-search.component';

describe('ChatSearchComponent', () => {
  let component: ChatSearchComponent;
  let fixture: ComponentFixture<ChatSearchComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ChatSearchComponent]
    });
    fixture = TestBed.createComponent(ChatSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
