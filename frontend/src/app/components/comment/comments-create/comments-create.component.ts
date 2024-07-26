import { HttpHeaders } from '@angular/common/http';
import { CommentService } from './../../../services/comment/comment.service';
import { Component, Input } from '@angular/core';
import { AuthService } from 'src/app/auth.service';
import { Post } from 'src/app/models/post.model';

@Component({
  selector: 'app-comments-create',
  templateUrl: './comments-create.component.html',
  styleUrls: ['./comments-create.component.css']
})
export class CommentsCreateComponent {
  commentContent: string = '';
  @Input() post: Post | undefined;
  message: string = '';

  constructor(private _AuthService:AuthService ,private commentService: CommentService) { }

  token:any = this._AuthService.getToken();

  headers: any = new HttpHeaders({
    Authorization: `Bearer ${this.token}`
  });

  createComment() {
    const commentData = { content: this.commentContent ,postId : this.post?.id }; 
    console.log(commentData);
    this.commentService.createComment(commentData,this.headers).subscribe(response => {
      console.log('Comment created successfully:', response);
    }, error => {
      console.error('Error creating comment:', error);
    });
  }

  checkInput(){

  }
}
