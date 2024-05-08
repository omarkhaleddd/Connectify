import { Component } from '@angular/core';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent {
  name =localStorage.getItem("displayName");
  isLogged:boolean=true;
  logout(){
    //removes token from the local storage and redirect to login page
  }
  editProfile(){
    //redirects to edit profile page with inputs to edit the profile
  }
}
