import { Injectable } from '@angular/core';
import { HttpTransportType, HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { UserDict } from 'src/app/models/UserDict.model';

@Injectable({
  providedIn: 'root',
})
export class StreamService {
  private hubConnection!: HubConnection;
  private userList = new Subject<UserDict>();
  userList$ = this.userList.asObservable();

  connectionId!: string;

  startConnection() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('https://localhost:7095/videoCallHub', {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets
      })
      .build();
  
    this.hubConnection.on('ReceiveConnectionId', (id: string) => {
      this.connectionId = id;
      console.log(this.connectionId);
      this.registerUser(sessionStorage.getItem('userSessionDisplayName') ?? '');
    });
  
    this.hubConnection.on('UserListUpdated', (users: UserDict) => {
      console.log(users);
      
      this.userList.next(users);
      console.log(this.userList);
    });
  
    this.hubConnection.start()
      .then(() => console.log("Connected"))
      .catch((err) => console.error('Error starting SignalR connection:', err));
  }

  registerUser(username: string) {
    this.hubConnection.invoke('RegisterUser', username);
  }

  sendOffer(targetUsername: string, offer: RTCSessionDescriptionInit) {
    console.log(targetUsername,offer);
    
    this.hubConnection.invoke('InitiateCall', targetUsername, JSON.stringify(offer))
    .catch(error => {
      console.error('Error invoking InitiateCall:', error);
      // Handle the error as needed (e.g., display a message to the user)
    });
  }

  sendAnswer(targetConnectionId: string, answer: RTCSessionDescriptionInit) {
    console.log(answer);
    
    this.hubConnection.invoke('SendAnswer', targetConnectionId,  JSON.stringify(answer));
  }

  sendIceCandidate(targetConnectionId: string, candidate: RTCIceCandidateInit) {
    this.hubConnection.invoke(
      'SendIceCandidate',
      targetConnectionId,
      JSON.stringify(candidate)
    );
  }

  onReceiveOffer(callback: (connectionId: string, offer: RTCSessionDescriptionInit) => void) {
    this.hubConnection.on('ReceiveOffer', callback);
  }

  onReceiveAnswer(callback: (connectionId: string,answer: RTCSessionDescriptionInit) => void) {
    this.hubConnection.on('ReceiveAnswer', callback);
  }

  onReceiveIceCandidate(callback: (connectionId: string,candidate: RTCIceCandidateInit) => void) {
    this.hubConnection.on('ReceiveIceCandidate', callback);
  }
}
