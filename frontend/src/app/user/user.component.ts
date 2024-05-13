import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth.service';
import { PostService } from '../services/post/post.service';
import { HttpHeaders } from '@angular/common/http';
import { Post } from '../models/post.model';
import { ActivatedRoute } from '@angular/router';

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

  constructor(private _AuthService:AuthService,private route: ActivatedRoute,private postService:PostService) { }

  ngOnInit(): void {
    
    this.getUserPosts()
    console.log(this.userPosts);
    console.log(this.displayName);
    
    
  }

  logout(){
    //removes token from the local storage and redirect to login page
  }
  editProfile(){
    //redirects to edit profile page with inputs to edit the profile
  }

  token:any = this._AuthService.getToken();

  headers: any = new HttpHeaders({
    Authorization: `Bearer ${this.token}`
  });


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
