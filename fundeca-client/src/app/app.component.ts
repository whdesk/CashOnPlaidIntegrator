import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'fundeca-client';

  constructor(private router: Router) {}

  goToPlaid() {
    this.router.navigate(['/plaid']);
  }
  goToStripe() {
    this.router.navigate(['/stripe']);
  }

  goToPaylandsTokenization() {
    this.router.navigate(['/paylands-tokenizacion']);
  }
}
