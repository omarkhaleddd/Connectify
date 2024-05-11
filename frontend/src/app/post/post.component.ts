import { AuthService } from 'src/app/auth.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PostService } from '../services/post/post.service';
import { Post } from '../models/post.model';
import { HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.css']
})
export class PostComponent implements OnInit {
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

