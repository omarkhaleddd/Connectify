import { Component, Input, OnInit } from '@angular/core';
import { Post } from 'src/app/models/post.model';

@Component({
  selector: 'app-post-get',
  templateUrl: './post-get.component.html',
  styleUrls: ['./post-get.component.css']
})
export class PostGetComponent {
  @Input() post: Post | undefined;

  likePost() {
    if (this.post) {
      this.post.likeCount++;
    }
  }

  commentPost() {
    if (this.post) {
      console.log('Commenting on post:', this.post.authorId);
    }
  }
}
