import { Injectable } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FormGroup } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private _HttpClient:HttpClient) { }

  getToken(): string | null {
    return localStorage.getItem('userToken');
  }

  getUserId(): string | null {
    return localStorage.getItem('userId');
  }


  register(data:FormGroup):Observable<any>{
    return this._HttpClient.post('https://localhost:7095/api/Accounts/Register',data);
  } 
  login(data:FormGroup):Observable<any>{
    return this._HttpClient.post('https://localhost:7095/api/Accounts/Login',data);
  }
}
