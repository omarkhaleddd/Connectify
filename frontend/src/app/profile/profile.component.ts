import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth.service';
import { PostService } from '../services/post/post.service';
import { HttpHeaders } from '@angular/common/http';
import { Post } from '../models/post.model';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  userId =localStorage.getItem("userId");
  displayName =localStorage.getItem("displayName");
  userPosts: Post[] = [];
  isLogged:boolean=true;

  constructor(private _AuthService:AuthService ,private postService:PostService) { }

  ngOnInit(): void {
    
    this.fetchUserPosts()
    console.log(this.userPosts);
    console.log(this.userId);
    
    
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


  fetchUserPosts() {
    const id = this.userId
    console.log(id);
    
    if (id) {
      
      this.postService.getPostsByAuthorId(id,this.headers).subscribe(posts => {
        this.userPosts = posts;
        console.log(this.userPosts)
      }, error => {
        console.error('Error fetching posts:', error);
      });
    }
  }
}
