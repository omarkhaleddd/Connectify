import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DataService {
  private sharedDataSubject = new BehaviorSubject<any>(null);
  sharedData$ = this.sharedDataSubject.asObservable();
  private sharedDataSubject2 = new BehaviorSubject<any>(null);
  sharedMessages$ = this.sharedDataSubject2.asObservable();
  constructor() {
    // Initialize shared data if needed
    this.sharedDataSubject.next({});
    this.sharedDataSubject2.next({});

  }

  updateSharedData(newData: any) {
    // Update shared data
    this.sharedDataSubject.next(newData);
  }
  updateSharedMessages(newData: any) {
    // Update shared data
    this.sharedDataSubject2.next(newData);    

  }
}