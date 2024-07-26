import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChatsListComponent } from './chats-list.component';

describe('ChatsListComponent', () => {
  let component: ChatsListComponent;
  let fixture: ComponentFixture<ChatsListComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ChatsListComponent]
    });
    fixture = TestBed.createComponent(ChatsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
