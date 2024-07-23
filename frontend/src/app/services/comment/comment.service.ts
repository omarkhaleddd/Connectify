import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CommentService {

  private apiUrl = 'https://localhost:7095/api/Comment/';

  constructor(private http: HttpClient) { }

  createComment(commentData: any,headers: any): Observable<any> {
    return this.http.post<Comment>(this.apiUrl + "AddComment", commentData,{headers : headers});
  }

}

