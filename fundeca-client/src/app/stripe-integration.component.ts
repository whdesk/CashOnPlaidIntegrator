import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { StripeIntegrationService } from './stripe-integration.service';

@Component({
  selector: 'app-stripe-integration',
  templateUrl: './stripe-integration.component.html',
  styleUrls: ['./stripe-integration.component.scss'],
  standalone: true,
  imports: [CommonModule, FormsModule]
})
export class StripeIntegrationComponent implements OnInit {
  paymentMethods: any[] = [];
  selectedPaymentMethodId: string | null = null;

  stripe: any;
  elements: any;
  cardElement: any;
  stripeCustomerId: string | null = null;
  paymentMethodId: string | null = null;
  stripeEmail: string = '';
  stripeName: string = '';
  loading = false;

  constructor(private StripeService: StripeIntegrationService) {}

  ngOnInit() {
    // El Card Element se inicializa después de crear el customer
    this.StripeService.getStripeObservable().subscribe(stripeInstance => {
      if (stripeInstance) {
        this.stripe = stripeInstance;
      }
    });
  }

  async createStripeCustomer() {
    this.loading = true;
    try {
      const res = await fetch('http://localhost:5199/api/stripe/create-customer', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email: this.stripeEmail, name: this.stripeName })
      });
      const data = await res.json();
      this.stripeCustomerId = data.customerId;
      // Inicializa Stripe Elements y el Card Element
     if(this.stripe) {
      console.log(this.stripe);

      this.elements = this.stripe.elements();
      this.cardElement = this.elements.create('card');
      const cardDiv = document.getElementById('card-element');
      if (cardDiv) this.cardElement.mount(cardDiv);
     }
      alert('Cliente creado en Stripe: ' + data.customerId);
    } catch (error) {
      alert('Error creando cliente en Stripe: ' + error);
    }
    this.loading = false;
  }

  async saveCardAndAttachToCustomer() {
    // ...
    // Después de asociar el método de pago correctamente:
    // this.fetchPaymentMethods();

    if (!this.stripe || !this.cardElement || !this.stripeCustomerId) {
      alert('Stripe.js o el cliente no está listo');
      return;
    }
    this.loading = true;
    const { paymentMethod, error } = await this.stripe.createPaymentMethod({
      type: 'card',
      card: this.cardElement,
      billing_details: { name: this.stripeName, email: this.stripeEmail }
    });
    if (error) {
      alert('Error creando método de pago: ' + error.message);
      this.loading = false;
      return;
    }
    this.paymentMethodId = paymentMethod.id;
    try {
      await fetch('http://localhost:5199/api/stripe/attach-payment-method', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          customerId: this.stripeCustomerId,
          paymentMethodId: this.paymentMethodId
        })
      });
      alert('Método de pago guardado y asociado correctamente.');
    } catch (error) {
      alert('Error asociando método de pago: ' + error);
    }
    this.loading = false;
  }

  async chargeCustomer() {
    if (!this.stripeCustomerId || !this.paymentMethodId) {
      alert('Primero crea el cliente y guarda el método de pago');
      return;
    }
    this.loading = true;
    try {
      const res = await fetch('http://localhost:5199/api/stripe/charge-off-session', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          customerId: this.stripeCustomerId,
          paymentMethodId: this.paymentMethodId,
          amount: 1000, // 10 USD (en centavos)
          currency: 'usd'
        })
      });
      const data = await res.json();
      alert('Cobro automático realizado. Status: ' + data.status);
    } catch (error) {
      alert('Error realizando el cobro automático: ' + error);
    }
    this.loading = false;
    this.fetchPaymentMethods(); // Refresca métodos de pago tras crear el cliente
  }

  fetchPaymentMethods() {
    if (!this.stripeCustomerId) return;
    fetch(`http://localhost:5199/api/stripe/payment-methods?customerId=${this.stripeCustomerId}`)
      .then(res => res.json())
      .then(data => this.paymentMethods = data);
  }

  selectPaymentMethod(id: string) {
    this.selectedPaymentMethodId = id;
  }

  onAddPaymentMethod() {
    // Mostrar UI para agregar método de pago (puedes mostrar el Card Element aquí)
    alert('Agregar método de pago (implementa la lógica aquí)');
  }

  onEditPaymentMethod(method: any) {
    // Lógica para editar método de pago (opcional)
    alert('Editar método de pago: ' + method.id);
  }
}

  