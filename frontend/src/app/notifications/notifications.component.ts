import { Component } from '@angular/core';
import { NotificationService } from '../services/notification/notification.service';
import { Notification } from '../models/notification.model';
import { HttpHeaders } from '@angular/common/http';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.css']
})
export class NotificationsComponent {
  // notifications: any[] = [
  //   { 
  //     content : "i love you",
  //     type : "mention" ,
  //     date : "2 hours ago"
  //   }
  // ];

  notifications: Notification[] = [];
  notificationsHistory: Notification[] = [];

    constructor(private notificationService: NotificationService , private _AuthService : AuthService) { }

    ngOnInit(): void {
      console.log(this.notifications);
      console.log(this.notifications);

      this.notificationService.connect().subscribe(() => {
        console.log('Connected to SignalR Hub');
        this.notificationService.onReceiveMessage((Notification) => {
          console.log(Notification);
          
          this.notifications.push(Notification);
          console.log(this.notifications);
          
        });
          }, error => {
        console.log('Error connecting to SignalR Hub:', error);
      });
      this.getMessages()
    }
    token:any = this._AuthService.getToken();

    headers: any = new HttpHeaders({
      Authorization: `Bearer ${this.token}`
    });
    
    getMessages(){
      this.notificationService.getNotificationHistory(this.headers).subscribe(notications => {
        // this.messages.push({ messages.recieverId, recieverName, message, sentAt })
        this.notificationsHistory =notications
        console.log(notications)  
      }, error => {
        console.error('Error fetching notifications:', error);
      });
}
}
