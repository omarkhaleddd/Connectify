import { friend } from './../../models/friend';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-chats-list',
  templateUrl: './chats-list.component.html',
  styleUrls: ['./chats-list.component.css'],
})
export class ChatsListComponent {
  @Input() friends : friend[] = [];
}
