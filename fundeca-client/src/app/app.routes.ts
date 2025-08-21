import { Routes } from '@angular/router';

import { PlaidIntegrationComponent } from './plaid-integration.component';
import { StripeIntegrationComponent } from './stripe-integration.component';
import { PaylandsTokenizationComponent } from './paylands-tokenization.component';

export const routes: Routes = [
  { path: 'plaid', component: PlaidIntegrationComponent },
  { path: 'stripe', component: StripeIntegrationComponent },
  { path: 'paylands-tokenizacion', component: PaylandsTokenizationComponent }
];
