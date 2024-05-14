import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private apiUrl = 'https://localhost:7095/api/Accounts/';

  constructor(private http: HttpClient) { }

  sendFriendRequest(userId: any,headers: any): Observable<any> {
    console.log(this.apiUrl + `/SendFriendRequest/${userId}`);
    
    return this.http.post<any>(this.apiUrl + `SendFriendRequest/${userId}`,null,{headers : headers});
  }

  checkFriendRequestFromUser(userId: any,headers: any): Observable<any> {
    console.log(this.apiUrl + `CheckFriendRequestFromUser/${userId}`);
    
    return this.http.get<any>(this.apiUrl + `CheckFriendRequestFromUser/${userId}`,{headers : headers});
  }
}
