//validators for validation
import { FormControl, FormGroup , Validators } from '@angular/forms';
import { Component } from '@angular/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  //form group -> form control
  registerForm = new FormGroup({
    name : new FormControl(null,[Validators.required,Validators.minLength(3),Validators.maxLength(10)]), //validation as a array in the arguments
    email: new FormControl(null,[Validators.required,Validators.email]),
    password: new FormControl(null,[Validators.required]),
    rePassword: new FormControl(null,[Validators.required])
  });

//functions here 
  registerUser(data: FormGroup){
    console.log(data.value);
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

