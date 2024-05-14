import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth.service';
import { PostService } from '../services/post/post.service';
import { HttpHeaders } from '@angular/common/http';
import { Post } from '../models/post.model';
import { ActivatedRoute } from '@angular/router';
import { UserService } from '../services/user/user.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  userId =localStorage.getItem("userId");
  displayName : string | undefined;
  userPosts: Post[] = [];
  isLogged:boolean=true;
  friendReq: string | undefined;
  hasRequested: boolean | undefined;

  constructor(private _AuthService:AuthService,private route: ActivatedRoute,private postService:PostService,private userService:UserService) { }

  ngOnInit(): void {
    
    this.getUserPosts()
    console.log(this.userPosts);
    this.checkFriendRequestFromUser();
    console.log(this.friendReq);

    if (!this.friendReq && this.hasRequested) {
      this.friendReq = "Send Friend Request";
    }else{
      this.friendReq = "Accept or Decline";
    }
    console.log(this.friendReq);
    // this.sendFriendRequest();
    
  }

  token:any = this._AuthService.getToken();

  headers: any = new HttpHeaders({
    Authorization: `Bearer ${this.token}`
  });

  logout(){

    console.log("erger");
    
    //removes token GrmPo{9n18|( from the local storage and redirect to login page
  }

  checkFriendRequestFromUser(){
    if (this.userId) {
      this.route.params.subscribe(params => {
        const userId = params['id'];
        console.log(userId);
        this.userService.checkFriendRequestFromUser(userId,this.headers).subscribe(res => {
          console.log(res)
          this.hasRequested = res.message;
        },error => {
          console.log(error);
        } );
      });
    }
  }

  sendFriendRequest() {
    if (this.userId) {
      this.route.params.subscribe(params => {
        const userId = params['id'];
        console.log(userId);
        this.userService.sendFriendRequest(userId,this.headers).subscribe(res => {
          console.log(res)
          this.friendReq = res.message;
        },error => {
          console.log(error);
        } );
      });
    }
  }


  getUserPosts() {
    this.route.params.subscribe(params => {
      const userId = params['id'];
      console.log(userId);
      this.postService.getPostsByAuthorId(userId,this.headers).subscribe(posts => {
        this.userPosts = posts;
        this.displayName = posts[0]?.authorName;
        console.log(posts[0].authorName);
        
        console.log(this.displayName)
      },error => {
        console.log(error);
      } );
    });
  }
}
