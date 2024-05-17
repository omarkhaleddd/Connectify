import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Post } from 'src/app/models/post.model';

@Injectable({
  providedIn: 'root'
})
export class PostService {
  private apiUrl = 'https://localhost:7095/api/Post/';

  constructor(private http: HttpClient) { }

  createPost(postData: any,headers: any): Observable<any> 
  {
    console.log(headers.get('Authorization'));
    return this.http.post<any>(this.apiUrl, postData,{headers : headers});
  }

  getAllPosts(headers: any): Observable<Post[]> {
    return this.http.get<Post[]>(this.apiUrl,{headers: headers});
  }

  getPost(id: number,headers: any): Observable<Post> {
    console.log(this.apiUrl + `${id}`);
    return this.http.get<Post>(this.apiUrl + `${id}`,{headers: headers});
  }

  getPostsByAuthorId(authorId: string, headers: any): Observable<Post[]> {
    console.log(this.apiUrl + `GetPostByAuthorId/${authorId}`);
    return this.http.get<Post[]>(this.apiUrl + `GetPostByAuthorId/${authorId}`, { headers: headers });
  }

  likePost(postId: number,headers: any): Observable<any> 
  {
    console.log(this.apiUrl + `LikePost/${postId}`);
    return this.http.put<any>(this.apiUrl + `LikePost/${postId}`,null,{headers : headers});
  } 

  createRepost(repostData: any,headers: any): Observable<any> 
  {
    console.log(headers.get('Authorization'));
    return this.http.post<any>(this.apiUrl + `Repost`, repostData,{headers : headers});
  }

  getAllReposts(headers: any): Observable<any[]> {
    
    return this.http.get<any[]>(this.apiUrl,{headers: headers});
  }
}
