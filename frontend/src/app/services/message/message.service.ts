import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { Message } from 'src/app/models/message';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  private apiUrl : string = "https://localhost:7095/api/Messages/"
  constructor(private http : HttpClient) { }
  
  getMessages(headers:any,id: string ):Observable<Message[]>{
    console.log(headers);
    return this.http.get<Message[]>(this.apiUrl + "getMessages/" + id, {headers: headers});
  }
  getGroupMessages(headers:any,groupName: string ):Observable<Message[]>{
    console.log(headers);
    return this.http.get<Message[]>(this.apiUrl + "getGroupMessages/" + groupName, {headers: headers});
  }
}
