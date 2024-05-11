import { SharedDataService } from './../../../services/sharedData/shared-data.service';
import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Post } from 'src/app/models/post.model';

@Component({
  selector: 'app-post-get',
  templateUrl: './post-get.component.html',
  styleUrls: ['./post-get.component.css']
})
export class PostGetComponent {
  @Input() post: Post | undefined;

  constructor(private router: Router, private dataService: SharedDataService) { }

  likePost() {
    if (this.post) {
      this.post.likeCount++;
    }
    
  }
  onPostClick(clickedPost: any) {
    this.dataService.setPostData(clickedPost); // Set the post data in DataService
    this.router.navigate(['/post/'+clickedPost.id]);
  }

  commentPost() {
    if (this.post) {
      console.log('Commenting on post:', this.post.authorId);
    }
  }
}
