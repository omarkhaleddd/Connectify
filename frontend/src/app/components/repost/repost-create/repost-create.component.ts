import { HttpHeaders } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { AuthService } from 'src/app/auth.service';
import { Post } from 'src/app/models/post.model';
import { PostService } from 'src/app/services/post/post.service';

@Component({
  selector: 'app-repost-create',
  templateUrl: './repost-create.component.html',
  styleUrls: ['./repost-create.component.css']
})
export class RepostCreateComponent implements OnInit{
  @Input() post : Post | undefined;
  repostContent: string = '';

  constructor(private _AuthService:AuthService ,private postService: PostService) { }

  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    console.log(this.post);
    
  }

  token:any = this._AuthService.getToken();

  headers: any = new HttpHeaders({
    Authorization: `Bearer ${this.token}`
  });

  createRepost() {
    console.log(this.repostContent);
    const repostData = { content: this.repostContent }; 
    this.postService.createRepost(repostData,this.headers).subscribe(response => {
      console.log(this.headers);
      
      console.log('Repost created successfully:', response);
    }, error => {
      console.error('Error creating repost:', error);
    });
  }
}
