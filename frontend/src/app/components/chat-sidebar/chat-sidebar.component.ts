import { HttpHeaders } from '@angular/common/http';
import { Component } from '@angular/core';
import { AuthService } from 'src/app/auth.service';
import { friend } from 'src/app/models/friend';
import { DataService } from 'src/app/services/data/data.service';
import { FriendService } from 'src/app/services/friends/friend.service';

@Component({
  selector: 'app-chat-sidebar',
  templateUrl: './chat-sidebar.component.html',
  styleUrls: ['./chat-sidebar.component.css']
})
export class ChatSidebarComponent {
  selectedFriendId: any | undefined;
  friends: friend[] = [];
  myUserId: string;
  public groupName: string | undefined;
  public isConnectedToGroup: boolean = false;

  constructor(
    private _AuthService: AuthService,
    private sharedData: DataService,
    private friendService: FriendService
  ) {
    this.myUserId = _AuthService.getUserId() || '';
  }
  ngOnInit(): void {
    this.fetchFriends();
  }
  token: any = this._AuthService.getToken();
  headers: any = new HttpHeaders({
    Authorization: `Bearer ${this.token}`,
  });
  public joinGroup() {
    if (this.groupName) {
      this.isConnectedToGroup = true;
      this.sharedData.updateGroup({
        isConnected: this.isConnectedToGroup,
        groupName: this.groupName,
      });
    }
  }
  
  fetchFriends() {
    this.friendService.getFriends(this.headers).subscribe(
      (friends) => {
        this.friends = friends;
        console.log(this.friends);
      },
      (error) => {
        console.log(`Error fetching friends :`, error);
      }
    );
  }

  onFriendSelected(friend: friend) {
    console.log('Selected friend:', friend);
  }
}
