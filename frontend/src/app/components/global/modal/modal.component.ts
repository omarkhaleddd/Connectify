import { Component, OnInit, Output, EventEmitter, Input, HostListener } from '@angular/core';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.css']
})
export class ModalComponent implements OnInit {
  @Output() closeModalEvent = new EventEmitter<boolean>();
  @Input() showTitle: boolean = true; // Default to true if not provided
  @Input() showCloseButton: boolean = true; 
  @Input()  modalBody: any;
  @Input() modalTitle: any;
  @Input() emptyMsg: any;
  @Input() users: any;

  constructor() { }

  ngOnInit(): void {
    console.log(this.modalBody);
    if (this.modalBody.length == 0) {
      this.modalBody = this.emptyMsg;
      console.log(this.modalBody);
    }
    console.log(this.users);
    
    if (this.users) {
      console.log(this.users);
      
    }

    console.log(this.modalBody);
    
    
  }
  

  closeModal() {
    this.closeModalEvent.emit();
  }



}
