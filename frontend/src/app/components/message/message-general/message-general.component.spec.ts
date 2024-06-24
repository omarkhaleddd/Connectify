import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MessageGeneralComponent } from './message-general.component';

describe('MessageGeneralComponent', () => {
  let component: MessageGeneralComponent;
  let fixture: ComponentFixture<MessageGeneralComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MessageGeneralComponent]
    });
    fixture = TestBed.createComponent(MessageGeneralComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
