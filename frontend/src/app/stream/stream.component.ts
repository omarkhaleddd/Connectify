import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-stream',
  templateUrl: './stream.component.html',
  styleUrls: ['./stream.component.css']
})
export class StreamComponent {
  selectedFriendData: any;
  selectedFriendId: any;

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.selectedFriendId = params['friendId'];
    });
  }

  onFriendSelected(friend: any) {
    console.log(friend);

    if (this.selectedFriendId === friend) {
      console.log("online");
      
    }
    
    this.selectedFriendData = friend;
    console.log(this.selectedFriendData);
    
  }
}
