import { Message } from 'src/app/models/message';
import { AuthService } from 'src/app/auth.service';
import { MessageService } from './../../../services/message/message.service';
import { friend } from 'src/app/models/friend';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { DataService } from 'src/app/services/data/data.service';
import { HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-message-friend',
  templateUrl: './message-friend.component.html',
  styleUrls: ['./message-friend.component.css']
})
export class MessageFriendComponent  implements OnInit{
  @Input() friend : friend | undefined;
  friendData:friend | undefined;
  sharedDataValue: any;
  messages: any;
  constructor(private dataService: DataService , private messageService:MessageService,private _AuthService:AuthService) {
    console.log(this.friend);
    // Subscribe to changes in shared data
    this.dataService.sharedData$.subscribe(data => {
    this.sharedDataValue = data;
    });
  }
  ngOnInit(): void {
    this.friendData = this.friend;
    console.log(this.friendData);
    
    
  }

  token:any = this._AuthService.getToken();

  headers: any = new HttpHeaders({
    Authorization: `Bearer ${this.token}`
  });
  clickFriend(newData:any){
    
    this.updateSharedData(newData);
    this.getMessages();
  }
  updateSharedData(newData:any) {
    // Update shared data through the service
    this.dataService.updateSharedData(newData);
  }
  updateSharedMessages(messages:any) {
    // Update shared data through the service    
    this.dataService.updateSharedMessages(messages);

  }
  getMessages(){
    console.log(this.friendData?.friendId);
    this.messageService.getMessages(this.headers,this.friendData?.friendId?? '').subscribe(messages => {
      // this.messages.push({ messages.recieverId, recieverName, message, sentAt })
      console.log(messages)
      this.updateSharedMessages(messages);

    }, error => {
      console.error('Error fetching posts:', error);
    });
  }
}
