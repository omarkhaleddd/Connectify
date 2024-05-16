import { Component } from '@angular/core';
import { NotificationService } from '../services/notification/notification.service';
import { Notification } from '../models/notification.model';

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

    constructor(private notificationService: NotificationService) { }

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
    }
}
