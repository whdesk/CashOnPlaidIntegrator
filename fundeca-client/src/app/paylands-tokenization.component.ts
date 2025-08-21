import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-paylands-tokenization',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './paylands-tokenization.component.html',
  styleUrls: ['./paylands-tokenization.component.scss']
})
export class PaylandsTokenizationComponent {
  customerToken: string | null = null;
  loading = false;
  error: string | null = null;
  savedCardData: any = null;

  private async loadPaylandsSdk(): Promise<void> {
    if ((window as any).paylands) return;
    await new Promise<void>((resolve, reject) => {
      const script = document.createElement('script');
      script.src = 'https://api.paylands.com/js/v1-iframe.js';
      script.type = 'text/javascript';
      script.onload = () => resolve();
      script.onerror = () => reject('No se pudo cargar el SDK de Paylands');
      document.body.appendChild(script);
    });
  }

  async createCustomer() {
    this.loading = true;
    this.error = null;
    try {
      await this.loadPaylandsSdk();
      // Llamada al backend para crear cliente y obtener token (mock por ahora)
      const response = await fetch('http://localhost:5199/api/PaylandsTokenization/create-customer', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ customer_ext_id: 'demo_' + Date.now() })
      });
      const data = await response.json();
      if (data.token) {
        this.customerToken = data.token;
        setTimeout(() => this.initPaylands(), 0);
      } else {
        this.error = 'No se pudo obtener el token de cliente';
      }
    } catch (err) {
      this.error = 'Error al crear cliente: ' + err;
    }
    this.loading = false;
  }

  initPaylands() {
    if (!(window as any).paylands || !this.customerToken) return;
    // Escuchar eventos del iframe
    window.addEventListener('message', this.receiveMessage.bind(this), false);
    // Sandbox solo para pruebas (quitar en producción)
    (window as any).paylands.setMode('sandbox');
    (window as any).paylands.setTemplate('F7CEE8DD-02BB-4AD6-AEFD-D5028B264C05');
    (window as any).paylands.initializate(this.customerToken, 'paylands-iframe');
  }

  onTokenizeCard() {
    this.error = null;
    this.loading = true;
    try {
      if ((window as any).paylands && this.customerToken) {
        (window as any).paylands.storeSourceCard();
      } else {
        this.error = 'SDK de Paylands no cargado o token faltante';
        this.loading = false;
      }
    } catch (e) {
      this.error = 'Error al invocar storeSourceCard: ' + e;
      this.loading = false;
    }
  }

  receiveMessage(event: MessageEvent) {
    if (event.data === 'paylandsLoaded') {
      // Ya inicializado en initPaylands
    } else if (event.data === 'error') {
      this.error = 'Error en la tarjeta';
      this.loading = false;
    } else if (event.data && event.data.savedCard) {
      this.savedCardData = event.data;
      this.loading = false;
    }
  }
}
