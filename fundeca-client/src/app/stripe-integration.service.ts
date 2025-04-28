import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class StripeIntegrationService {
  private stripe: any;
  private stripeReady = new BehaviorSubject<any>(null);

  constructor() {
    if (!(window as any).Stripe) {
      const script = document.createElement('script');
      script.src = 'https://js.stripe.com/v3/';
      script.async = true;
      script.onload = () => {
        this.stripe = (window as any).Stripe('pk_test_51RHEGARU1mpnIbwDI1GKivZ7fADLI1wqWf3qNNAVP2YfUViUwzGcHYVfUXtrpgf5kcsRdUcI34fwRP0qFn4WocOr000u7XWgN8'); // Reemplaza por tu clave publicable
        this.stripeReady.next(this.stripe);
        console.log(this.stripe);
      };
      document.body.appendChild(script);
    } else {
      this.stripe = (window as any).Stripe('pk_test_51RHEGARU1mpnIbwDI1GKivZ7fADLI1wqWf3qNNAVP2YfUViUwzGcHYVfUXtrpgf5kcsRdUcI34fwRP0qFn4WocOr000u7XWgN8');
      this.stripeReady.next(this.stripe);
    }
  }

  getStripeObservable() {
    return this.stripeReady.asObservable();
  }

  getStripe() {
    return this.stripe;
  }
}
