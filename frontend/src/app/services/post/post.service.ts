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

  createPost(postData: any,headers: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, postData,{headers : headers});
  }

  getAllPosts(): Observable<Post[]> {
    return this.http.get<Post[]>(this.apiUrl);
  }
}