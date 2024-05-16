import { friend } from 'src/app/models/friend';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { DataService } from 'src/app/services/data/data.service';

@Component({
  selector: 'app-message-friend',
  templateUrl: './message-friend.component.html',
  styleUrls: ['./message-friend.component.css']
})
export class MessageFriendComponent {
  @Input() friend : friend | undefined;
  sharedDataValue: any;
  constructor(private dataService: DataService) {
    // Subscribe to changes in shared data
    this.dataService.sharedData$.subscribe(data => {
      this.sharedDataValue = data;
    });
  }

  updateSharedData(newData:any) {
    // Update shared data through the service
    this.dataService.updateSharedData(newData);
  }
    selectFriend(friend: friend | undefined) {
    }
}
