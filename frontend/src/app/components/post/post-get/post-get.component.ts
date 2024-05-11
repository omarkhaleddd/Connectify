import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Post } from 'src/app/models/post.model';

@Component({
  selector: 'app-post-get',
  templateUrl: './post-get.component.html',
  styleUrls: ['./post-get.component.css']
})
export class PostGetComponent {
  @Input() post: Post | undefined;

  constructor(private router: Router) { }

  likePost() {
    if (this.post) {
      this.post.likeCount++;
    }
    
  }
  onPostClick(clickedPost: any) {
    this.router.navigate(['/post/'+clickedPost.id]);
  }

  commentPost() {
    if (this.post) {
      console.log('Commenting on post:', this.post.authorId);
    }
  }
}
