import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from 'src/app/models/user';

@Injectable({
  providedIn: 'root'
})
export class SearchService {
  private apiUrl = 'https://localhost:7095/api/Accounts/Search/';

  constructor(private http: HttpClient) { }

  searchUser(Query: any,headers: any): Observable<User[]> {
    return this.http.get<User[]>(this.apiUrl + Query,{headers : headers});
  }

}
