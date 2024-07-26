import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css'],
})
export class ChatComponent {
  selectedFriendId: number | undefined;

  constructor(private router: Router) {}

  onFriendSelected(friendId: any) {
    
    this.selectedFriendId = friendId;
    this.router.navigate([], {
      queryParams: { friendId: this.selectedFriendId },
      queryParamsHandling: 'merge', 
    });
  }
}
