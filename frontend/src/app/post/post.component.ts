import { Component } from '@angular/core';
import { Subscription } from 'rxjs';
import { SharedDataService } from '../services/sharedData/shared-data.service';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.css']
})
export class PostComponent {
  postData: any;
  private subscription: Subscription = new Subscription(); 

  constructor(private dataService: SharedDataService) { }

  ngOnInit() {
    this.subscription = this.dataService.postData$.subscribe(data => {
      this.postData = data;
      console.log('Received data:', this.postData);
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe(); // Unsubscribe to avoid memory leaks
  }
}

