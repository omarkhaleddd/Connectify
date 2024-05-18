import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MailService {
  apiUrl: string = "https://localhost:7095/api/Mail/";
  constructor(private http:HttpClient) {
   }
   sendMail(data:any,headers: any ):Observable<any>{
    console.log(data);
    
    return  this.http.post(this.apiUrl + "SendMail"+data,null,{headers:headers})
   }
}
