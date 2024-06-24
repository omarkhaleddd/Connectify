import { friend } from './../../models/friend';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FriendService {
  private apiUrl : string = "https://localhost:7095/api/Accounts/"
  constructor(private http : HttpClient) { }
  
  getFriends(headers:any):Observable<friend[]>{
    return this.http.get<friend[]>(this.apiUrl + "Friends", {headers: headers});
  }
  
}
