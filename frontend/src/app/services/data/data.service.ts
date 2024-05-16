import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DataService {
  private sharedDataSubject = new BehaviorSubject<any>(null);
  sharedData$ = this.sharedDataSubject.asObservable();

  constructor() {
    // Initialize shared data if needed
    this.sharedDataSubject.next({});
  }

  updateSharedData(newData: any) {
    // Update shared data
    this.sharedDataSubject.next(newData);
  }
}