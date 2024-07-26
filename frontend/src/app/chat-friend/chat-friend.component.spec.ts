import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChatFriendComponent } from './chat-friend.component';

describe('ChatFriendComponent', () => {
  let component: ChatFriendComponent;
  let fixture: ComponentFixture<ChatFriendComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ChatFriendComponent]
    });
    fixture = TestBed.createComponent(ChatFriendComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
