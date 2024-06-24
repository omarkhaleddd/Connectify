import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';
import { MailService } from '../services/mail/mail.service';
import { HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  errorMsg='';
  isLoading :boolean = false;
  loginForm = new FormGroup({
    email: new FormControl(null,[Validators.required,Validators.email]),
    password: new FormControl(null,[Validators.required])
  });
  constructor(private _AuthService:AuthService,private _Router:Router,private _mailService:MailService){}

  loginUser(data:FormGroup){
    console.log(data.value);
    this.isLoading = true;
    this._AuthService.login(data.value).subscribe({
      next: (res) => {  
        console.log(res);
        if(res.token){
          sessionStorage.setItem('userSessionId',res.id);
          sessionStorage.setItem('userSessionToken',res.token);
          sessionStorage.setItem('userSessionDisplayName',res.displayName);
          localStorage.setItem('userToken',res.token);
          localStorage.setItem('displayName',res.displayName);
          localStorage.setItem('userId',res.id);
          this.sendMail();
          this._Router.navigate(['/home']);
        }
      },
      error: (myErrors) => { 
        this.isLoading=false;
        console.log(myErrors);
        this.errorMsg=myErrors},
      complete: () => {this.isLoading=false},

    })
  }
  getErrorMessage(controlName: string): string {
    const control = this.loginForm.get(controlName);
    if (control?.hasError('required')) {
      return 'This field is required.';
    } else if (control?.hasError('email')) {
      return 'Invalid email format.';
    } else {
      return '';
    }
  }
  //send mail service
  sendMail(){
    var token = this._AuthService.getToken();

    var headers: any = new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
    var userMail=this.loginForm.value.email;

    this._mailService.sendMail(userMail,headers).subscribe(
      response =>{
        console.log('Mail sent successfully:', response);
      },
      error => {
        console.error('Error Sending Mail:', error);
      });
  }
}
