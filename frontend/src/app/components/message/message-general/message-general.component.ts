import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-message-general',
  templateUrl: './message-general.component.html',
  styleUrls: ['./message-general.component.css']
})
export class MessageGeneralComponent implements OnInit{
  selectedFriendId: string | undefined;
  public isConnectedToGroup: boolean = false;

  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    console.log(this.selectedFriendId);
    
  }

  onFriendSelected(friendId: any) {
    console.log(friendId.target.value);
    console.log(friendId);
    
    
    this.selectedFriendId = friendId;
    console.log('Selected Friend ID:', friendId);
  }
}
