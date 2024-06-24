import { Component, Input } from '@angular/core';
import { Repost } from '../models/repost.model';
import { ActivatedRoute } from '@angular/router';
import { Post } from '../models/post.model';
import { PostService } from '../services/post/post.service';
import { AuthService } from '../auth.service';
import { HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-repost',
  templateUrl: './repost.component.html',
  styleUrls: ['./repost.component.css']
})
export class RepostComponent {
  post: Post | undefined;


  constructor(private route: ActivatedRoute, private postService: PostService,private _AuthService:AuthService ) { }

  ngOnInit(): void {
    this.getPost();
  }

  token:any = this._AuthService.getToken();

  headers: any = new HttpHeaders({
    Authorization: `Bearer ${this.token}`
  });

  getPost() {
    this.route.params.subscribe(params => {
      const postId = +params['id'];
      console.log(postId);
      this.postService.getPost(postId,this.headers).subscribe(result => {
        this.post = result;
        console.log('Post:', this.post);
      },error => {
        console.log(error);
      } );
    });
  }
}
