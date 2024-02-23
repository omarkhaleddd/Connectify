import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';

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
  constructor(private _AuthService:AuthService,private _Router:Router){}

  loginUser(data:FormGroup){
    console.log(data.value);
    this.isLoading = true;
    this._AuthService.login(data.value).subscribe({
      next: (res) => {  
        console.log(res);
        if(res.message == "success"){
          localStorage.setItem('userToken',res.token);
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
}
