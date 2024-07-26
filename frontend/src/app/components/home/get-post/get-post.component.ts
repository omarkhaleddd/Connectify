import { HttpHeaders } from '@angular/common/http';
import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/auth.service';
import { Post } from 'src/app/models/post.model';
import { PostService } from 'src/app/services/post/post.service';

@Component({
  selector: 'app-get-post',
  templateUrl: './get-post.component.html',
  styleUrls: ['./get-post.component.css']
})
export class GetPostComponent {
  donationAmount: any = 0;
  submitDonation() {
    throw new Error('Method not implemented.');
  }

  
  reportPost(arg0: Post | undefined) {
    throw new Error('Method not implemented.');
  }
  deletePost(arg0: Post | undefined) {
    throw new Error('Method not implemented.');
  }
  editPost(arg0: Post | undefined) {
    throw new Error('Method not implemented.');
  }
  @Input() post: Post | undefined;
  isLiked: string | undefined;
  likeCount: number | undefined;
  isLikedBtn: any;
  likes: any;
  emptyMsg: string = 'No likes';
  showModal = false;
  showDropDown = false;
  rePost: any;

  isModalVisible: boolean = false;
  modalButtons = [
    { label: 'Close', action: () => this.closeModal() },
    { label: 'Save', action: () => this.saveChanges() },
  ];

  openModal() {
    this.showDropDown = false;
    this.isModalVisible = true;
    
  }

  closeModal() {
    this.isModalVisible = false;
  }

  saveChanges() {
    this.donateToPost(this.post)
    console.log('Changes saved!');
    this.closeModal();
  }

  constructor(
    private _AuthService: AuthService,
    private postService: PostService,
    private router: Router
  ) {}

  ngOnInit(): void {
    console.log(this.post);
    console.log(this.post?.comments);
    
    var userId = this._AuthService.getUserId();
    this.isLikedBtn = this.post?.likes?.some((like) => like.userId === userId);
    console.log(this.post);
    this.likeCount = this.post?.likes?.length;
    this.likes = this.post?.likes?.map((obj) => obj.userName);

    if (this.isLikedBtn) {
      this.isLiked = 'Dislike';
    } else {
      this.isLiked = 'Like';
    }

    this.rePost = this.post?.post;
  }

  donateToPost(post: Post | undefined) {
    console.log(this.donationAmount);
    this.router.navigate(['/donation/' + post?.id], { queryParams: { donAmount: this.donationAmount } });
  }

  token: any = this._AuthService.getToken();

  headers: any = new HttpHeaders({
    Authorization: `Bearer ${this.token}`,
  });

  likePost() {
    if (this.post) {
      console.log(this.post.likes,"hkkhkj");
      this.postService.likePost(this.post.id, this.headers).subscribe(
        (res) => {
          this.isLiked = res.message;
          console.log(this.likeCount);

          this.likeCount = res.likeCount;
          console.log(res);
        },
        (error) => {
          console.error('Error liking post:', error);
        }
      );
    }
  }

  onPostClick(clickedPost: any) {
    this.router.navigate(['/post/' + clickedPost.id]);
  }

  commentPost() {
    if (this.post) {
      console.log('Commenting on post:', this.post.authorId);
    }
  }

  repost(clickedPost: any) {
    //repost post
    this.router.navigate(['/repost/' + clickedPost.id]);
  }

  toggleDropdown() {
    this.showDropDown = !this.showDropDown;
  }
}
