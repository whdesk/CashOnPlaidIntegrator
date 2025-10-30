import { Routes } from '@angular/router';

import { PlaidIntegrationComponent } from './plaid-integration.component';
import { StripeIntegrationComponent } from './stripe-integration.component';
import { PaylandsTokenizationComponent } from './paylands-tokenization.component';
import { SignalrLoginComponent } from './components/signalr-login/signalr-login.component';
import { MoneiTokenizationComponent } from './monei-tokenization.component';
import { PaycometTokenizationComponent } from './paycomet-tokenization.component';

export const routes: Routes = [
  { 
    path: 'signalr', 
    component: SignalrLoginComponent,
    title: 'SignalR Connection'
  },
  { 
    path: 'plaid', 
    component: PlaidIntegrationComponent,
    title: 'Plaid Integration'
  },
  { 
    path: 'stripe', 
    component: StripeIntegrationComponent,
    title: 'Stripe Integration'
  },
  { 
    path: 'paylands-tokenizacion', 
    component: PaylandsTokenizationComponent,
    title: 'Paylands Tokenization'
  },
  { 
    path: 'monei-tokenizacion', 
    component: MoneiTokenizationComponent,
    title: 'MONEI Tokenization'
  },
  { 
    path: 'paycomet-tokenizacion', 
    component: PaycometTokenizationComponent,
    title: 'PayCOMET Tokenization'
  },
  { 
    path: '', 
    redirectTo: '/signalr', 
    pathMatch: 'full' 
  }
];
