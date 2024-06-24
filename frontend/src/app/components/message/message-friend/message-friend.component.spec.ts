import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MessageFriendComponent } from './message-friend.component';

describe('MessageFriendComponent', () => {
  let component: MessageFriendComponent;
  let fixture: ComponentFixture<MessageFriendComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MessageFriendComponent]
    });
    fixture = TestBed.createComponent(MessageFriendComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
