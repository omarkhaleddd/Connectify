import { AuthService } from './../auth.service';
//validators for validation
import { FormControl, FormGroup , Validators } from '@angular/forms';
import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  errorMsg = '';
  isLoading:boolean =false;
  //form group -> form control

  registerForm = new FormGroup({
    email: new FormControl(null,[Validators.required,Validators.email]),
    DisplayName : new FormControl(null,[Validators.required,Validators.minLength(3),Validators.maxLength(10)]), //validation as a array in the arguments
    PhoneNumber: new FormControl(null,[Validators.required]),
    password: new FormControl(null,[Validators.required])
  });
  constructor(private _AuthService:AuthService , private _Router:Router){}
//functions here 
  registerUser(data: FormGroup){
    console.log(data.value);
    this.isLoading=true;
    this._AuthService.register(data.value).subscribe({
      next: (res)=>{ 
        //navigate to login
        console.log(res);
        if(res.token){
          localStorage.setItem('userToken',res.token);
          this._Router.navigate(['/login']);
        }
      },
      error: (myErrors)=>{ 
        this.isLoading= false;
        console.log(myErrors);
        this.errorMsg=myErrors;
        //show it in html
      },
      complete: ()=> { this.isLoading = false}
    });
  }
  getErrorMessage(controlName: string): string {
    const control = this.registerForm.get(controlName);
    if (control?.hasError('required')) {
      return 'This field is required.';
    } else if (control?.hasError('minlength')) {
      return 'Must be at least 3 characters long.';
    } else if (control?.hasError('maxlength')) {
      return 'Cannot exceed 10 characters.';
    } else if (control?.hasError('email')) {
      return 'Invalid email format.';
    } else if (control?.hasError('pattern')) {
      return 'Invalid format.';
    } else {
      return '';
    }
  }
}

