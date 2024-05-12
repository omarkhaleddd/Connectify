import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
  query : string = '';
  
  constructor(private router: Router) { }

  search(){
    console.log(this.query);
   this.router.navigate(['/search/'+ this.query]);
  }
}