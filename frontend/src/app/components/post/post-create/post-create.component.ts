import { HttpHeaders } from '@angular/common/http';
import { Component } from '@angular/core';
import { AuthService } from 'src/app/auth.service';
import { PostService } from 'src/app/services/post/post.service';
import { SearchService } from 'src/app/services/search/search.service';

@Component({
  selector: 'app-post-create',
  templateUrl: './post-create.component.html',
  styleUrls: ['./post-create.component.css']
})
export class PostCreateComponent {
  postContent: string = '';
  tagContent: string = '';
  potTaggedUsers: any;
  searchResults: any[] = [];
  mentionArr: string[] = [];
  modalTitle: string = "Mentions";
  modalBody : any ;
  emptyMsg: any;
  showSearchComponent: boolean = false;

  constructor(private _AuthService:AuthService,private searchService:SearchService ,private postService: PostService) { }

  token:any = this._AuthService.getToken();

  headers: any = new HttpHeaders({
    Authorization: `Bearer ${this.token}`
  });



  createPost() {
    console.log(this.postContent);
    console.log(this.mentionArr);
    const postData = { content: this.postContent, mentions: this.mentionArr }; 
    console.log(postData);
    
    this.postService.createPost(postData,this.headers).subscribe(response => {
      console.log(this.headers);
      
      console.log('Post created successfully:', response);
    }, error => {
      console.error('Error creating post:', error);
    });
  }

  // onPostContentChange(e:any){
  //   console.log(e.target.value);
  //   this.tagContent = e.target.value
  //   if (this.tagContent.includes('@')) {
  //     const searchText = this.tagContent.split('@').pop(); 
      
  //     if (searchText) {
  //       console.log(searchText[searchText.length - 1]);
  //       if (searchText[searchText.length - 1] === ' ') {
  //         console.log(searchText[searchText.length - 1]);
  //         this.mentionArr.push(searchText)
          
  //         this.showSearchComponent = false;
  //         return;

  //       }
  //       this.searchUser(searchText)
  //     }
      
  //     this.showSearchComponent = true;
  //   } else {
  //     this.potTaggedUsers = [];
  //     this.showSearchComponent = false;
  //   }
  //   console.log(this.showSearchComponent);
    
  // }

onPostContentChange(e:any){
  this.tagContent = e.target.value;
    const atIndex = this.tagContent.lastIndexOf('@');

    if (atIndex !== -1) {
      const searchText = this.tagContent.slice(atIndex + 1);

      if (searchText.trim()) {
        if (searchText.endsWith(' ')) {
          this.mentionArr.push(searchText.trim());
          this.showSearchComponent = false;
        } else {
          this.showSearchComponent = true;
          this.searchUser(searchText.trim());
        }
      } else {
        this.showSearchComponent = false;
        this.searchResults = [];
      }
    } else {
      this.showSearchComponent = false;
      this.searchResults = [];
    }
  }


  searchUser(query: string){
    this.searchService.searchUser(query,this.headers).subscribe(result => {
      this.searchResults = result;
      console.log('Users:', this.searchResults);
      // this.modalBody = this.users
      console.log(this.searchResults);
      
    },error => {
      console.log(error);
    });
  }

}
