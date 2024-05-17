import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HttpTransportType, HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Observable, Subject } from 'rxjs';
import { Notification } from 'src/app/models/notification.model';

@Injectable({
    providedIn: 'root'
})
export class NotificationService {
    private hubConnection: HubConnection;
    private notificationSubject: Subject<Notification> = new Subject<Notification>();
    private apiUrl = 'https://localhost:7095/notification';
    private recieveNotificationsUrl : string = "https://localhost:7095/api/Notification/Notifications"

    constructor(private http : HttpClient) {
        this.hubConnection = new HubConnectionBuilder()
            .withUrl(this.apiUrl,{
              skipNegotiation: true,
              transport: HttpTransportType.WebSockets})
            .build();

        // this.hubConnection.start()
        //     .catch(err => console.log(err));

        // this.hubConnection.on('SendMessage', (message: Notification) => {
        //   console.log("connected");
          
        //     this.notificationSubject.next(message);
        // });
    }

    public connect(): Observable<void> {
      return new Observable<void>(observer => {
        this.hubConnection.start()
          .then(() => {
            observer.next();
            observer.complete();
          })
          .catch(error => { 
            console.log(error);
            observer.error(error);
          });
      });
    }

    public onReceiveMessage(callback: (message: Notification) => void) {
      this.hubConnection.on('SendNotification', callback);
    }

    getNotifications() {
        return this.notificationSubject.asObservable();
    }
    getNotificationHistory(headers:any):Observable<Notification[]>{
      console.log(headers);
      return this.http.get<Notification[]>(this.recieveNotificationsUrl, {headers: headers});
    }
    // public startConnection = () => {
    //   this.hubConnection = new signalR.HubConnectionBuilder()
    //                           .withUrl('https://localhost:7132/Notify',{ skipNegotiation: true,
    //                           transport: signalR.HttpTransportType.WebSockets})
    //                           .build();
    //   this.hubConnection
    //     .start()
    //     .then(() => console.log('Connection started'))
    //     .catch(err => console.log('Error while starting connection: ' + err))
    // }
    
    // public addProductListner = () => {
    //   this.hubConnection.on('SendMessage', (notification: Notification) => {
    //     this.showNotification(notification);
    //     this.productService.get();
    //   });
    // }

    // showNotification(notification: Notification) {
    //   this.toastr.warning( notification.message,notification.productID+" "+notification.productName);
    // }
}
