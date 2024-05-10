import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/auth.service';
import { Post } from 'src/app/models/post.model';

@Component({
  selector: 'app-posts-display',
  templateUrl: './posts-display.component.html',
  styleUrls: ['./posts-display.component.css']
})
export class PostsDisplayComponent implements OnInit {
  posts: Post[] = [];

  constructor(private _AuthService:AuthService ,private http: HttpClient) { }

  ngOnInit(): void {
    this.fetchPosts();
  }
  token:any = this._AuthService.getToken();

  headers: any = new HttpHeaders({
    Authorization: `Bearer ${this.token}`
  });

  fetchPosts() {
    this.http.get<Post[]>('https://localhost:7095/api/Post/', { headers: this.headers }).subscribe(posts => {
      this.posts = posts;
      console.log(this.posts)
    }, error => {
      console.error('Error fetching posts:', error);
    });
  }
}
