import { Component, Input } from '@angular/core';
import { Post } from 'src/app/models/post.model';

@Component({
  selector: 'app-posts-profile-display',
  templateUrl: './posts-profile-display.component.html',
  styleUrls: ['./posts-profile-display.component.css']
})
export class PostsProfileDisplayComponent {
  @Input() userPosts : Post[] = [];
  
}
