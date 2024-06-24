import { Router } from '@angular/router';
import { Component, Input } from '@angular/core';
import { User } from 'src/app/models/user.model';

@Component({
  selector: 'app-search-user',
  templateUrl: './search-user.component.html',
  styleUrls: ['./search-user.component.css']
})
export class SearchUserComponent {
  @Input() user: User | undefined;
  adlyPage: string = "user/";
  constructor(private router: Router){}

  getUserById(){
    console.log(this.user?.id);
    this.router.navigate([this.adlyPage+this.user?.id]);
  }
}
