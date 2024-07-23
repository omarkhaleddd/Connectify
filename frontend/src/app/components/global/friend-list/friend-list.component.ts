import { HttpHeaders } from '@angular/common/http';
import { Component, EventEmitter, Output } from '@angular/core';
import { AuthService } from 'src/app/auth.service';
import { FriendService } from 'src/app/services/friends/friend.service';
import { StreamService } from 'src/app/services/streaming/stream.service';

@Component({
  selector: 'app-friend-list',
  templateUrl: './friend-list.component.html',
  styleUrls: ['./friend-list.component.css'],
})
export class FriendListComponent {
  @Output() friendSelected = new EventEmitter<any>();
  userId = sessionStorage.getItem('userSessionId');
  token = sessionStorage.getItem('userSessionToken');
  userName = sessionStorage.getItem('userSessionDisplayName');
  friends: string[] = [];
  userList: any;
  showConn: boolean = false;
  // friends : any = [];

  // constructor(private friendService:FriendService){}

  // friends = [
  //   { id: '01e3cf1e-863b-4c10-b5f4-3606e9ddc900', name: 'red' },
  //   { id: 'e1fe5c61-60c0-4aa0-909b-9678605b507b', name: 'omarAdly' },
  //   // Add more friends as needed
  // ].filter(f => f.id !== this.userId);

  // headers : any = new HttpHeaders({
  //   Authorization : `Bearer ${this.token}`
  // });

  // getFriends(){
  //   console.log(this.headers);

  //   this.friendService.getFriends(this.headers).subscribe(
  //     friends =>{
  //       this.friends = friends;
  //       console.log(this.friends)
  //     }, error => {
  //       console.log(`Error fetching friends :` , error);
  //     }
  //   )
  // }

  constructor(private streamService: StreamService) {}

  ngOnInit() {
    console.log(this.showConn);
    // console.log(this.userConn);
    
    // if (this.userConn) {
      this.streamService.userList$.subscribe((users) => {
        this.userList = users;
        console.log(this.showConn);

        if (this.userName) {          
          this.showConn = true;
          this.friends = Object.keys(users).filter((f) => f !== this.userName);
          console.log(this.friends);
        } else {
          this.showConn = false;
        }
      });
    }

  //}

  selectFriend(friend: string) {
    const selectedFriend = this.userList[friend];
    console.log(this.userList);
    
    this.friendSelected.emit(selectedFriend);
  }
}
