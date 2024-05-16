import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MessageChatComponent } from './message-chat.component';

describe('MessageChatComponent', () => {
  let component: MessageChatComponent;
  let fixture: ComponentFixture<MessageChatComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MessageChatComponent]
    });
    fixture = TestBed.createComponent(MessageChatComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
