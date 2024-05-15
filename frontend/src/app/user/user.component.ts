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
  
  friendState: string | undefined;

  constructor(private _AuthService:AuthService,private route: ActivatedRoute,private postService:PostService,private userService:UserService) { }

  ngOnInit(): void {
    
    this.getUserPosts()
    console.log(this.userPosts);
    this.checkFriendRequestFromUser();
    console.log(this.friendReq);
    console.log(this.hasRequested);

    

    if (!this.friendReq) {
      this.friendReq = "Send Friend Request";
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
          console.log(res.message)
          this.hasRequested = res.message;
          console.log(this.hasRequested);
          if (this.hasRequested){
            this.friendReq = undefined;
          }
          console.log(this.friendReq);
          
        },error => {
          console.log(error);
        } );
      });
    }
  }

  blockUser(){
    if (this.userId) {
      this.route.params.subscribe(params => {
        const userId = params['id'];
        console.log(userId);
        this.userService.checkFriendRequestFromUser(userId,this.headers).subscribe(res => {
          console.log(res.message)
          this.hasRequested = res.message;
          console.log(this.hasRequested);
          if (this.hasRequested){
            this.friendReq = undefined;
          }
          console.log(this.friendReq);
          
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

  answerRequest(state:number){
    if (this.userId) {
      this.route.params.subscribe(params => {
        const userId = params['id'];
        console.log(userId);
        this.userService.answerRequest(userId,state,this.headers).subscribe(res => {
          console.log(res)
          this.friendState = res.message
          if (this.friendState == "Rejected") {
            console.log("You Declined this request");
          } else if (this.friendState == "Accepted"){
            console.log("You accepted this request");
          }else{
            console.log(res);
            
          }

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
