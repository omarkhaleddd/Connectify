import { HttpHeaders } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/auth.service';
import { Likes } from 'src/app/models/like';
import { Post } from 'src/app/models/post.model';
import { PostService } from 'src/app/services/post/post.service';

@Component({
  selector: 'app-post-get',
  templateUrl: './post-get.component.html',
  styleUrls: ['./post-get.component.css']
})
export class PostGetComponent implements OnInit{
  @Input() post: Post | undefined;
  isLiked : string | undefined;
  likeCount : number | undefined;
  isLikedBtn : any ;
  likes: any;
  emptyMsg: string = "No likes" ;
  showModal = false;
  rePost : any;
  

  constructor(private _AuthService:AuthService ,private postService:PostService,private router:Router) 
  { 

  }

  ngOnInit(): void {
    var userId =this._AuthService.getUserId()
    this.isLikedBtn = this.post?.likes.some(like => like.userId === userId);
    console.log(this.post);
    this.likeCount = this.post?.likeCount
    this.likes = this.post?.likes.map(obj => obj.userName)
    
    if(this.isLikedBtn){
      this.isLiked = "Dislike";
    }
    else{
      this.isLiked = "Like";
    }

    this.rePost = this.post?.post
  }

  token:any = this._AuthService.getToken();

  headers: any = new HttpHeaders({
    Authorization: `Bearer ${this.token}`
  });

  likePost() {
    if (this.post) {
      console.log(this.post.likes);
      this.postService.likePost(this.post.id,this.headers).subscribe(res => {
        this.isLiked = res.message; 
        console.log(this.likeCount);
        
        this.likeCount = res.likeCount;
        console.log(res);
        
      }, error => {
        console.error('Error liking post:', error);
      }
    );
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

  repost(clickedPost: any){
    //repost post
    this.router.navigate(['/repost/'+clickedPost.id]);
  }
}
