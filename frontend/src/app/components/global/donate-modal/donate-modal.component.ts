import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-donate-modal',
  templateUrl: './donate-modal.component.html',
  styleUrls: ['./donate-modal.component.css']
})
export class DonateModalComponent {
  @Input() isVisible: boolean = false;
  @Input() title: string | null = null;
  @Input() showFooter: boolean = true;
  @Input() buttons: { label: string, action: Function }[] = [];

  closeModal() {
    this.isVisible = false;
  }
}
