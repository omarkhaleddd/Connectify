import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MessageFriendListSidebarComponent } from './message-friend-list-sidebar.component';

describe('MessageFriendListSidebarComponent', () => {
  let component: MessageFriendListSidebarComponent;
  let fixture: ComponentFixture<MessageFriendListSidebarComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MessageFriendListSidebarComponent]
    });
    fixture = TestBed.createComponent(MessageFriendListSidebarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
