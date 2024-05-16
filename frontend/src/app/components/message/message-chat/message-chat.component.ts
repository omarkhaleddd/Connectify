import { friend } from 'src/app/models/friend';
import { AuthService } from 'src/app/auth.service';
import { SignalrService } from './../../../services/signalr/signalr.service';
import { Component, Input, OnInit } from '@angular/core';
import { DataService } from 'src/app/services/data/data.service';

@Component({
  selector: 'app-message-chat',
  templateUrl: './message-chat.component.html',
  styleUrls: ['./message-chat.component.css']
})
export class MessageChatComponent implements OnInit {
  userId: any = '' ; // Replace with your user ID
  displayName: any = ''; // Replace with your display name
  message: string = '';
  messages: any[] = [];
  sharedDataValue: any;
  @Input() selectedFriendId: string | undefined;

  constructor(private _AuthService: AuthService ,private dataService: DataService, private chatService: SignalrService) {
    this.dataService.sharedData$.subscribe(data => {
      this.sharedDataValue = data;
      console.log(this.sharedDataValue);
      
    })}

  ngOnInit() {
    console.log(this.selectedFriendId);
    if(this._AuthService.getUserId()==this.sharedDataValue.friendId){
      this.userId = this.sharedDataValue.userId;
      this.displayName =this.sharedDataValue.userName;
    }
    else{
      this.userId = this.sharedDataValue.friendId;
      this.displayName =this.sharedDataValue.friendName;
    }
    console.log(this.userId);
    console.log(this.displayName);
    this.chatService.connect().subscribe(() => {
      console.log('Connected to SignalR Hub');
      this.chatService.onReceiveMessage((userId, displayName, message, sentAt) => {

        this.messages.push({ userId, displayName, message, sentAt });
        console.log(this.messages);
        
      });
        }, error => {
      console.log('Error connecting to SignalR Hub:', error);
    });

  }

  sendMessage() {
    console.log("here");

    if (this.message) {
      console.log("ana");
      var id ;
      var displayName;
      console.log(this._AuthService.getUserId());
      console.log(this.sharedDataValue.friendId);

      if(this._AuthService.getUserId()==this.sharedDataValue.friendId){
        id = this.sharedDataValue.userId;
        displayName =this.sharedDataValue.userName;
      
      }
      else{
        id = this.sharedDataValue.friendId;
        displayName =this.sharedDataValue.friendName;
        console.log("here");
        
      }
      console.log(displayName);
      console.log(id);      
      if (this.message && this.chatService.isConnected()) {  // Check connection before sending

      this.chatService.sendMessage(id, displayName, this.message)
        .then(() => {
          this.message = '';
          console.log("sent");
          
        })
        .catch(error => {
          console.log(error);
        });
    }
   else {
    console.log("Not connected to SignalR Hub. Cannot send message.");
  }
  }}
}
