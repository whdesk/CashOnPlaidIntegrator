import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

declare const monei: any;

@Component({
  selector: 'app-monei-tokenization',
  templateUrl: './monei-tokenization.component.html',
  styleUrls: ['./monei-tokenization.component.scss'],
  standalone: true,
  imports: [CommonModule, FormsModule]
})
export class MoneiTokenizationComponent implements OnInit, OnDestroy {
  accountId: string = '';
  sessionId: string = '';
  paymentId: string = '';
  usingPaymentId: boolean = false;
  loading = false;
  token: string | null = null;
  error: string | null = null;
  // Option B fields
  amount: number = 110;
  currency: string = 'EUR';
  orderId: string = '';
  callbackUrl: string = 'https://example.com/monei/callback';
  savedPaymentToken: string | null = null; // permanente para cobros futuros

  private cardInput: any = null;

  ngOnInit(): void {
    console.log('[MONEI] MoneiTokenizationComponent loaded');
    // Mensaje visible para verificar render
    if (!this.token && !this.error) {
      this.error = 'Vista MONEI cargada. Introduce accountId o paymentId y pulsa Inicializar.';
    }
  }

  private generateOrderId() {
    const rand = Math.random().toString(36).slice(2, 8).toUpperCase();
    this.orderId = `ORDER-${Date.now()}-${rand}`;
  }

  initCardInput() {
    this.destroyCardInput();
    if (this.usingPaymentId) {
      if (!this.paymentId) {
        this.error = 'Falta paymentId';
        return;
      }
    } else {
      if (!this.accountId) {
        this.error = 'Falta accountId';
        return;
      }
    }
    this.error = null;

    try {
      const options: any = this.usingPaymentId
        ? { paymentId: this.paymentId }
        : { accountId: this.accountId, sessionId: this.sessionId || undefined };

      if (typeof monei === 'undefined' || !monei.CardInput) {
        this.error = 'monei.js no está cargado. Asegúrate de incluir https://js.monei.com/v2/monei.js en index.html';
        return;
      }

      this.cardInput = monei.CardInput(options);
      this.cardInput.render('#monei-card-input');
    } catch (e: any) {
      this.error = e?.message || 'Error inicializando CardInput';
    }
  }

  async tokenize() {
    if (!this.cardInput) {
      this.error = 'CardInput no está inicializado';
      return;
    }
    this.loading = true;
    this.token = null;
    this.error = null;
    try {
      const result = await monei.createToken(this.cardInput);
      if (result?.error) {
        this.error = result.error;
      } else if (result?.token) {
        this.token = result.token;
      } else {
        this.error = 'No se recibió token';
      }
    } catch (e: any) {
      this.error = e?.message || 'Error generando token';
    } finally {
      this.loading = false;
    }
  }

  // Opción B: crear Payment en backend sin token, obtener paymentId e inicializar CardInput con ese id
  async createPaymentWithoutToken() {
    try {
      this.loading = true;
      this.token = null;
      this.error = null;
      if (!this.orderId) {
        this.generateOrderId();
      }
      const res = await fetch('http://localhost:5199/api/monei/create-payment', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          amount: this.amount,
          currency: this.currency,
          orderId: this.orderId,
          description: 'Pago de prueba',
          callbackUrl: this.callbackUrl
        })
      });
      const data = await res.json();
      if (!res.ok) {
        this.error = data?.error || JSON.stringify(data);
        return;
      }
      if (data?.id) {
        this.paymentId = data.id;
        this.usingPaymentId = true;
        this.initCardInput();
      } else {
        this.error = 'No se recibió paymentId en la respuesta';
      }
    } catch (e: any) {
      this.error = e?.message || 'Error creando Payment en backend';
    } finally {
      this.loading = false;
    }
  }

  // Confirmar en backend y solicitar token permanente para cobros futuros
  async confirmPaymentAndSaveCard() {
    try {
      this.loading = true;
      this.error = null;
      this.savedPaymentToken = null;
      if (!this.paymentId) {
        this.error = 'No hay paymentId. Crea primero el Payment.';
        return;
      }
      if (!this.token) {
        if (!this.cardInput) {
          this.error = 'CardInput no está inicializado';
          return;
        }
        const result = await monei.createToken(this.cardInput);
        if (result?.error) {
          this.error = result.error;
          return;
        }
        this.token = result?.token || null;
      }
      if (!this.token) {
        this.error = 'No se pudo obtener token para confirmar el pago';
        return;
      }

      const res = await fetch('http://localhost:5199/api/monei/confirm-payment', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          paymentId: this.paymentId,
          paymentToken: this.token,
          sessionId: this.sessionId || undefined,
          generatePaymentToken: true
        })
      });
      const data = await res.json();
      if (!res.ok) {
        this.error = data?.error || JSON.stringify(data);
        return;
      }
      this.savedPaymentToken = data?.paymentToken || null;
      if (!this.savedPaymentToken) {
        console.log('[MONEI] Confirm response', data);
      }
    } catch (e: any) {
      this.error = e?.message || 'Error confirmando el pago en backend';
    } finally {
      this.loading = false;
    }
  }

  ngOnDestroy(): void {
    this.destroyCardInput();
  }

  private destroyCardInput() {
    try {
      if (this.cardInput && typeof this.cardInput.destroy === 'function') {
        this.cardInput.destroy();
      }
    } catch {}
    this.cardInput = null;
  }
}
