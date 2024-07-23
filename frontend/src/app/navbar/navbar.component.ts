import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from './../auth.service';
import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SearchService } from '../services/search/search.service';
import { FormBuilder } from '@angular/forms';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit{
  query: string = '';
  searchResults: any[] = [];
  queryForm = this.fb.nonNullable.group({
    query: '',
  })
  modalTitle: string = "Recents";
  modalBody : any ;
  emptyMsg: any;

  
  constructor(private router:Router,private fb:FormBuilder,private authService:AuthService,private searchService:SearchService) { }

  ngOnInit(): void {
    this.emptyMsg = "no searches";

    console.log(this.modalBody);
    console.log(this.query);
    
  }

  onInputChange(e: any): void {
    this.query = e.target.value;
    if (this.query.trim() === '') {
      this.searchResults = [];
      return;
    }
    
    console.log(this.query);
    this.searchUser(this.query);
  }

  search() : void{
    console.log(this.query);
    // this.searchUser();

    console.log(this.query);
    
    this.searchUser(this.query)
    console.log(this.searchResults);
    
    this.router.navigate(['/search/'+ this.query]);
  }

  showModal: boolean = false;

  token:any = this.authService.getToken();

  headers: any = new HttpHeaders({
    Authorization: `Bearer ${this.token}`
  });
  
  searchUser(query: string){
    this.searchService.searchUser(query,this.headers).subscribe(result => {
      this.searchResults = result;
      console.log('Users:', this.searchResults);
      // this.modalBody = this.users
      console.log(this.searchResults);
      
    },error => {
      console.log(error);
    });
  }
}