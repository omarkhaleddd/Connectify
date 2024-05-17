import { MessageService } from './../../../services/message/message.service';
import { friend } from 'src/app/models/friend';
import { AuthService } from 'src/app/auth.service';
import { SignalrService } from './../../../services/signalr/signalr.service';
import { Component, Input, OnInit } from '@angular/core';
import { DataService } from 'src/app/services/data/data.service';
import { HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-message-chat',
  templateUrl: './message-chat.component.html',
  styleUrls: ['./message-chat.component.css']
})
export class MessageChatComponent implements OnInit {
  userId: any = '' ; // Replace with your user ID
  recieverName: any = ''; // Replace with your display name
  message: string = '';
  messages: any[] = [];
  sharedDataValue: any;
  sharedMessages:any;
  @Input() selectedFriendId: string | undefined;

  
  constructor(private _AuthService: AuthService,private _messageService:MessageService ,private dataService: DataService, private chatService: SignalrService) {
    this.dataService.sharedData$.subscribe(data => {
      this.sharedDataValue = data;
      console.log(this.sharedDataValue);
      
    })
    this.dataService.sharedMessages$.subscribe(messages => {
      this.sharedMessages = messages;
      console.log(this.sharedMessages);
    })
  }

  ngOnInit() {
    console.log(this.selectedFriendId);
    if(this._AuthService.getUserId()==this.sharedDataValue.friendId){
      this.userId = this.sharedDataValue.userId;
      this.recieverName =this.sharedDataValue.userName;
    }
    else{
      this.userId = this.sharedDataValue.friendId;
      this.recieverName =this.sharedDataValue.friendName;
    }    
    console.log(this.userId);
    console.log(this.recieverName);
    this.chatService.connect().subscribe(() => {
      console.log('Connected to SignalR Hub');
      this.chatService.onReceiveMessage((recieverId, recieverName, message, sentAt) => {

        this.messages.push({ recieverId, recieverName, message, sentAt });
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
      var recieverName;
      console.log(this._AuthService.getUserId());
      console.log(this.sharedDataValue.friendId);
      var authId = this._AuthService.getUserId() || "null";
      if(this._AuthService.getUserId()==this.sharedDataValue.friendId){
        id = this.sharedDataValue.userId;
        recieverName =this.sharedDataValue.userName;
      
      }
      else{
        id = this.sharedDataValue.friendId;
        recieverName =this.sharedDataValue.friendName;
        console.log("here");
        
      }
      console.log(recieverName);
      console.log(id);      
      if (this.message && this.chatService.isConnected()) {  // Check connection before sending

      this.chatService.sendMessage(authId,id , this.message)
        .then(() => {
          this.message = '';
          console.log("sent");
          
        })
        .catch(error => {
          console.log(error);
        });
    }
   else {
    console.log("Not connected to SignalR Hub. Cannot send message. !");
  }
  }}

}
