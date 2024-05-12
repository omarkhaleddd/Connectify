import { HttpHeaders } from '@angular/common/http';
import { Component } from '@angular/core';
import { AuthService } from 'src/app/auth.service';
import { PostService } from 'src/app/services/post/post.service';

@Component({
  selector: 'app-post-create',
  templateUrl: './post-create.component.html',
  styleUrls: ['./post-create.component.css']
})
export class PostCreateComponent {
  postContent: string = '';

  constructor(private _AuthService:AuthService ,private postService: PostService) { }

  token:any = this._AuthService.getToken();

  headers: any = new HttpHeaders({
    Authorization: `Bearer ${this.token}`
  });

  createPost() {
    console.log(this.postContent);
    const postData = { content: this.postContent }; 
    this.postService.createPost(postData,this.headers).subscribe(response => {
      console.log(this.headers);
      
      console.log('Post created successfully:', response);
    }, error => {
      console.error('Error creating post:', error);
    });
  }
}
