import { PaymentService } from './../services/payment/payment.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { loadStripe, Stripe } from '@stripe/stripe-js';
import { AuthService } from '../auth.service';
import { HttpHeaders } from '@angular/common/http';
import { environment } from '../enviroment';

@Component({
  selector: 'app-donation',
  templateUrl: './donation.component.html',
  styleUrls: ['./donation.component.css'],
})
export class DonationComponent implements OnInit {
  stripe: Stripe | null = null;
  card: any;
  donationAmount: number = 0; 
  queryParams:any;
  postId:any;
  clientSecret:string = '' ;

  constructor(private route: ActivatedRoute,private paymentService:PaymentService,private _AuthService:AuthService,private router: Router){}

  ngOnInit() {

    this.route.params.subscribe(params => {
      this.postId = params['id'];
      console.log(this.postId);
      
    });

    this.route.queryParams.subscribe(params => {
      // this.queryParams = params['donAmount'];
      console.log(params);
      this.donationAmount = params['donAmount']
    });

    this.setupStripe()
  }

  token:any = this._AuthService.getToken();

  headers: any = new HttpHeaders({
    Authorization: `Bearer ${this.token}`
  });

  async setupStripe(){
    this.stripe = await loadStripe(environment.stripePublicKey);
    const elements = this.stripe?.elements();
    this.card = elements?.create('card');
    this.card?.mount('#card-element');
    this.card?.on('change', (event:any) => {
      const displayError = document.getElementById('card-errors');
      if (event.error) {
        displayError!.textContent = event.error.message;
      } else {
        displayError!.textContent = '';
      }
    });
  }

  async pay() {
    this.paymentService.createPaymentIntent(this.donationAmount,this.postId,this.headers).subscribe(result => {
      this.clientSecret = result.clientSecret;
      console.log('clientSecret: ', this.clientSecret);
    },error => {
      console.log(error);
    } );

    console.log(this.clientSecret);
    
    
    if (this.clientSecret) {
      const stripeConfirm =  this.stripe?.confirmCardPayment(this.clientSecret, {
        payment_method: {
          card: this.card,
        },
      }).catch(error => {
        console.log(error);
      }
      );
    }
      
      console.log("payment success!");
      this.router.navigate(['/'])
  }

}
