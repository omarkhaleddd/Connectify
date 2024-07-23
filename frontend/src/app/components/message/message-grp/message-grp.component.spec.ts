import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MessageGrpComponent } from './message-grp.component';

describe('MessageGrpComponent', () => {
  let component: MessageGrpComponent;
  let fixture: ComponentFixture<MessageGrpComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MessageGrpComponent]
    });
    fixture = TestBed.createComponent(MessageGrpComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
