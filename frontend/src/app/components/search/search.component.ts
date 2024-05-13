import { HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/auth.service';
import { User } from 'src/app/models/user.model';
import { SearchService } from 'src/app/services/search/search.service';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})

export class SearchComponent implements OnInit {
  users: User[] = [];

  constructor(private route: ActivatedRoute, private searchService: SearchService,private authService:AuthService ) { }

  ngOnInit(): void {
    this.searchUser();
  }

  token:any = this.authService.getToken();

  headers: any = new HttpHeaders({
    Authorization: `Bearer ${this.token}`
  });
  searchUser(){
    this.route.params.subscribe(params => {
      const query = params['query'];
      console.log(query);
      this.searchService.searchUser(query,this.headers).subscribe(result => {
        this.users = result;
        console.log('Users:', this.users);
      },error => {
        console.log(error);
      });
    });
  }
}
