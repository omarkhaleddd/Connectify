import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SharedDataService {
  
  private postDataSubject = new BehaviorSubject<any>(null);
  postData$ = this.postDataSubject.asObservable();

  constructor() { }

  setPostData(data: any) {
    this.postDataSubject.next(data);
  }
}
