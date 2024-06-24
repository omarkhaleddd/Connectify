import { HttpHeaders } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/auth.service';
import { Likes } from 'src/app/models/like';
import { Repost } from 'src/app/models/repost.model';
import { PostService } from 'src/app/services/post/post.service';

@Component({
  selector: 'app-repost-get',
  templateUrl: './repost-get.component.html',
  styleUrls: ['./repost-get.component.css']
})
export class RepostGetComponent  implements OnInit{
  @Input() repost: Repost | undefined;
  isLiked : string | undefined;
  likeCount : number | undefined;
  isLikedBtn : any ;
  likes: any;
  emptyMsg: string = "No likes" ;
  showModal = false;
  

  constructor(private _AuthService:AuthService ,private postService:PostService,private router:Router) 
  { 

  }

  ngOnInit(): void {
    // var userId =this._AuthService.getUserId()
    // this.isLikedBtn = this.repost?.likes.some(like => like.userId === userId);
    // console.log(this.repost);
    // this.likeCount = this.repost?.likeCount
    // this.likes = this.repost?.likes.map(obj => obj.userName)
    
    // if(this.isLikedBtn){
    //   this.isLiked = "Dislike";
    // }
    // else{
    //   this.isLiked = "Like";
    // }
  }

  token:any = this._AuthService.getToken();

  headers: any = new HttpHeaders({
    Authorization: `Bearer ${this.token}`
  });

  // likePost() {
  //   if (this.repost) {
  //     console.log(this.repost.likes);
  //     this.postService.likePost(this.repost.id,this.headers).subscribe(res => {
  //       this.isLiked = res.message; 
  //       console.log(this.likeCount);
        
  //       this.likeCount = res.likeCount;
  //       console.log(res);
        
  //     }, error => {
  //       console.error('Error liking post:', error);
  //     }
  //   );
  //   }
  // }

  onRepostClick(clickedPost: any) {
    this.router.navigate(['/repost/'+clickedPost.id]);
  }

  commentPost(clickedPost: any) {
      this.router.navigate(['/repost/'+clickedPost.id]);
  }

}
