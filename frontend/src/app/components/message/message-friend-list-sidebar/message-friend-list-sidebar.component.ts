import { DataService } from 'src/app/services/data/data.service';
import { SignalrService } from './../../../services/signalr/signalr.service';
import { HttpHeaders } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AuthService } from 'src/app/auth.service';
import { friend } from 'src/app/models/friend';
import { FriendService } from 'src/app/services/friends/friend.service';

@Component({
  selector: 'app-message-friend-list-sidebar',
  templateUrl: './message-friend-list-sidebar.component.html',
  styleUrls: ['./message-friend-list-sidebar.component.css']
})
export class MessageFriendListSidebarComponent implements OnInit {
  selectedFriendId: any | undefined;
  friends : friend[] = [];
  public groupName: string|undefined; 
  myUserId:string;
  public isConnectedToGroup: boolean = false;

  constructor(private _AuthService : AuthService,private sharedData:DataService , private friendService : FriendService) {
    this.myUserId = _AuthService.getUserId()|| "";
   }
  ngOnInit(): void {
    this.fetchFriends();

  }
  token:any = this._AuthService.getToken();
  headers : any = new HttpHeaders({
    Authorization : `Bearer ${this.token}`
  });

  fetchFriends(){
    this.friendService.getFriends(this.headers).subscribe(
      friends =>{
        this.friends = friends;
        console.log(this.friends)
      }, error => {
        console.log(`Error fetching friends :` , error);
      }
    )
  }

  onFriendSelected(friend: friend) {
    console.log("Selected friend:", friend); // Access friend properties directly
  }
  public joinGroup() {
    if (this.groupName) {
      this.isConnectedToGroup = true;
      this.sharedData.updateGroup({isConnected:this.isConnectedToGroup , groupName : this.groupName} )
    }
  }

}
