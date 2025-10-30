import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet, 
    RouterLink, 
    RouterLinkActive
  ],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'CashOnPlaid Integrator';

  constructor(private router: Router) {}
  
  goToPaylandsTokenization() {
    this.router.navigate(['/paylands-tokenization']);
  }

  goToPlaid() {
    this.router.navigate(['/plaid-integration']);
  }

  goToSignalR() {
    this.router.navigate(['/signalr-login']);
  }

  goToStripe() {
    this.router.navigate(['/stripe-integration']);
  }
}
