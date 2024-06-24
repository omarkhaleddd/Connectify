import { Component } from '@angular/core';

@Component({
  selector: 'app-stream',
  templateUrl: './stream.component.html',
  styleUrls: ['./stream.component.css']
})
export class StreamComponent {
  selectedFriendData: any;

  onFriendSelected(friend: any) {
    console.log(friend);

    //Z3ydkRUD5IoMrjhSenm2Ng  user 1
    
    this.selectedFriendData = friend;
    console.log(this.selectedFriendData);
    
  }
}
