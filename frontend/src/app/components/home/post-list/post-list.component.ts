import { HttpHeaders } from '@angular/common/http';
import { Component, Input } from '@angular/core';
import { AuthService } from 'src/app/auth.service';
import { Post } from 'src/app/models/post.model';
import { PostService } from 'src/app/services/post/post.service';

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrls: ['./post-list.component.css']
})
export class PostListComponent {
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
