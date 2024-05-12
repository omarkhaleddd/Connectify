import { HttpHeaders } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/auth.service';
import { Post } from 'src/app/models/post.model';
import { PostService } from 'src/app/services/post/post.service';

@Component({
  selector: 'app-post-get',
  templateUrl: './post-get.component.html',
  styleUrls: ['./post-get.component.css']
})
export class PostGetComponent {
  @Input() post: Post | undefined;
  isLiked : string = "Like";

  constructor(private _AuthService:AuthService ,private postService:PostService,private router:Router) { }

  token:any = this._AuthService.getToken();

  headers: any = new HttpHeaders({
    Authorization: `Bearer ${this.token}`
  });


  likePost() {
    if (this.post) {
      console.log(this.post.id);
      console.log(this.headers);
      if (this.headers.has('Authorization')) {
        console.log('Token was sent successfully:', this.headers);
      } else {
        console.log('Token was not sent in the headers.');
      }
      
      this.postService.likePost(this.post.id,this.headers).subscribe(res => {
        console.log(res)
        this.isLiked = res;
        console.log(this.isLiked);  
      }, error => {
        console.log(error);
        console.error('Error liking post:', error);
      });
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
