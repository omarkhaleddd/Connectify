import { Injectable } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FormGroup } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private _HttpClient:HttpClient) { }

  register(data:FormGroup):Observable<any>{
    return this._HttpClient.post('https://route-ecommerce.onrender.com/api/v1/auth/signup',data);
  } 
  login(data:FormGroup):Observable<any>{
    return this._HttpClient.post('https://route-ecommerce.onrender.com/api/v1/auth/signup',data);
  }
}
