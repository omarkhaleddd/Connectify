import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { retry, catchError } from 'rxjs/operators';
import { HttpTransportType, HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  public connection: HubConnection;
  private baseUrl = 'https://localhost:7095/chatHub'; 

  constructor(private http: HttpClient) {
    this.connection = new HubConnectionBuilder()
      .withUrl(this.baseUrl,{
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets
      })
      .build();
  }

  public connect(): Observable<void> {
    return new Observable<void>(observer => {
      this.connection.start()
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

  public isConnected(): boolean {
    return this.connection && this.connection.state === HubConnectionState.Connected;
  }

  public async sendMessage(userId: string, displayName: string, message: string): Promise<void> {
    if (this.isConnected()) {
      await this.connection.invoke('Send', userId, displayName, message);
      console.log(message);
      
    } else {
      throw new Error('Not connected to SignalR Hub');
    }
  }

  public onReceiveMessage(callback: (userId: string, displayName: string, message: string, sentAt: Date) => void) {
    console.log(callback);
    this.connection.on('RecieveMessage', callback);
  }
  public joinGroup(groupName: string, userId: string) {
    this.connection.invoke('JoinGroup', groupName, userId)
      .catch(err => console.log(err));
  }

  public sendMessageToGroup(groupName: string, senderId: string, message: string) {
    this.connection.invoke('SendMessageToGroup', groupName, senderId, message)
      .catch(err => console.log(err));
  }

public addReceiveMessageListener(callback: (senderId: string, senderName: string, message: string,sentAt: Date) => void) {
this.connection.on('recieveMessage', callback);
}
}