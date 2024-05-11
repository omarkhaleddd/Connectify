import { SharedDataService } from './../../../services/sharedData/shared-data.service';
import { HttpHeaders } from '@angular/common/http';
import { CommentService } from './../../../services/comment/comment.service';
import { Component } from '@angular/core';
import { AuthService } from 'src/app/auth.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-comments-create',
  templateUrl: './comments-create.component.html',
  styleUrls: ['./comments-create.component.css']
})
export class CommentsCreateComponent {
  commentContent: string = '';
  postData: any;
  private subscription: Subscription = new Subscription(); 

  constructor(private _AuthService:AuthService ,private commentService: CommentService, private dataService:SharedDataService) { }

  ngOnInit() {
    this.subscription = this.dataService.postData$.subscribe(data => {
      this.postData = data;
      console.log('Received data:', this.postData);
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe(); // Unsubscribe to avoid memory leaks
  }

  token:any = this._AuthService.getToken();

  headers: any = new HttpHeaders({
    Authorization: `Bearer ${this.token}`
  });

  createComment() {
    const commentData = { content: this.commentContent ,postId : this.postData.id }; 
    console.log(commentData);
    this.commentService.createComment(commentData,this.headers).subscribe(response => {
      console.log('Comment created successfully:', response);
    }, error => {
      console.error('Error creating comment:', error);
    });
  }
}
