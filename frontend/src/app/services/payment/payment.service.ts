import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Post } from 'src/app/models/post.model';
import { PaymentIntent } from '@stripe/stripe-js';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {
  private apiUrl = 'https://localhost:7095/api/Payment/';

  constructor(private http: HttpClient) { }

  createPaymentIntent(amount: number, postId: number, headers: any) : Observable<any> {
    return this.http.post(this.apiUrl + "create-payment-intent", { Amount: amount, PostId: postId }, { headers: headers });
  }

  
}
