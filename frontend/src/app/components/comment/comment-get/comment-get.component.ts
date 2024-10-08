import { Component, Input } from '@angular/core';
import { Comment } from 'src/app/models/comment.model';

@Component({
  selector: 'app-comment-get',
  templateUrl: './comment-get.component.html',
  styleUrls: ['./comment-get.component.css']
})
export class CommentGetComponent {

  @Input() comment: Comment | undefined;

  likeComment() {
    if (this.comment) {
      this.comment.likeCount++;
    }
  }

  commentPost() {
    if (this.comment) {
      console.log('Commenting on post:', this.comment.authorId);
    }
  }
}
