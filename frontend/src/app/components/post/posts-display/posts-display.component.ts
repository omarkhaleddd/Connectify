import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/auth.service';
import { Post } from 'src/app/models/post.model';
import { PostService } from 'src/app/services/post/post.service';

@Component({
  selector: 'app-posts-display',
  templateUrl: './posts-display.component.html',
  styleUrls: ['./posts-display.component.css']
})
export class PostsDisplayComponent implements OnInit {
  
  posts: Post[] = [];

  constructor(private _AuthService:AuthService ,private postService:PostService) { }

  ngOnInit(): void {
    this.fetchPosts()
  }

  token:any = this._AuthService.getToken();

  headers: any = new HttpHeaders({
    Authorization: `Bearer ${this.token}`
  });

  fetchPosts() {
    this.postService.getAllPosts(this.headers).subscribe(posts => {
      this.posts = posts;
      console.log(this.posts)
    }, error => {
      console.error('Error fetching posts:', error);
    });
  }
}
