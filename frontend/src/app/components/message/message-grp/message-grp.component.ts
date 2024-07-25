import { HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/auth.service';
import { DataService } from 'src/app/services/data/data.service';
import { MessageService } from 'src/app/services/message/message.service';
import { SignalrService } from 'src/app/services/signalr/signalr.service';

@Component({
  selector: 'app-message-grp',
  templateUrl: './message-grp.component.html',
  styleUrls: ['./message-grp.component.css']
})
export class MessageGrpComponent implements OnInit {
  userId: any = '' ; // Replace with your user ID
  recieverName: any = ''; // Replace with your display name
  message: string = '';
  messages: any[] = [];
  sharedDataValue: any;
  sharedMessages:any;
  isConnected:boolean = true;
  GroupData:any ;
  isUserId: string;
  grpMessages: any;
  FirstTime:boolean = true;
  constructor(private _AuthService: AuthService,private _messageService:MessageService ,private dataService: DataService, private chatService: SignalrService) {
    this.dataService.sharedData$.subscribe(data => {
      this.sharedDataValue = data;
      console.log(this.sharedDataValue);
      
    })
    this.dataService.sharedMessages$.subscribe(messages => {
      this.sharedMessages = messages;
      console.log(this.sharedMessages);


    })
    this.dataService.sharedGroup$.subscribe(group => {
      this.GroupData = group;
      console.log(this.GroupData);
    })

    this.isUserId = this._AuthService.getUserId() || "";
    console.log(this.isUserId);
    
  }


  ngOnInit(): void {
    this.chatService.connect().subscribe(() => {
      console.log('Connected to SignalR Hub');

    
    // this.chatService.onReceiveMessage((recieverId, recieverName, message, sentAt) => {

    //   this.messages.push({ recieverId, recieverName, message, sentAt });
    //   console.log(this.messages);
      
    // });
    this.joinGroup();
    this.getGroupMessages()
    if (!this.grpMessages) {
      this.grpMessages = [];
  }
    console.log(this.grpMessages);
    this.chatService.addReceiveMessageListener((userId, displayName, message, sentAt) => {
      console.log('Received message:', { userId, displayName, message,sentAt });
      this.grpMessages.push({ userId, displayName, message , sentAt});
      console.log(this.grpMessages);
  });
        }, error => {
      console.log('Error connecting to SignalR Hub:', error);
    });

    
  }

  token:any = this._AuthService.getToken();

  headers: any = new HttpHeaders({
    Authorization: `Bearer ${this.token}`
  });
  
   public joinGroup() {
    if (this.GroupData.groupName) {
      console.log(this.GroupData.groupName);
      
      this.chatService.joinGroup(this.GroupData.groupName, this.isUserId);
      this.isConnected = true;
    }
  }
  updateSharedData(newData:any) {
    // Update shared data through the service
    this.dataService.updateSharedData(newData);
  }
  updateSharedMessages(messages:any) {
    // Update shared data through the service    
    this.dataService.updateSharedMessages(messages);
  }

  getGroupMessages(){
    this._messageService.getGroupMessages(this.headers,this.GroupData.groupName).subscribe(messages => {
      // this.messages.push({ messages.recieverId, recieverName, message, sentAt })
      console.log(messages)
      this.grpMessages = messages;
      this.updateSharedMessages(messages);
      this.sendMessageToGroup();
    }, error => {
      console.error('Error fetching messages:', error);
    });
    this.updateSharedMessages(this.messages);
  }

  public sendMessageToGroup() {   
    if(this.FirstTime){
      this.joinGroup();
      this.FirstTime = false;
    }     
    if (this.isConnected) {
      this.chatService.sendMessageToGroup(this.GroupData.groupName, this.isUserId, this.message);      
      this.message = '';
    }
  }

}
