import { Routes } from '@angular/router';

import { PlaidIntegrationComponent } from './plaid-integration.component';
import { StripeIntegrationComponent } from './stripe-integration.component';

export const routes: Routes = [
  { path: 'plaid', component: PlaidIntegrationComponent },
  { path: 'stripe', component: StripeIntegrationComponent }
];
