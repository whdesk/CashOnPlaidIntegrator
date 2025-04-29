import { Component, OnInit, NgZone } from '@angular/core';
import { StripeIntegrationService } from './stripe-integration.service';

@Component({
  selector: 'app-plaid-integration',
  imports: [ ], // FormsModule debe agregarse en el módulo principal
  templateUrl: './plaid-integration.component.html',
  styleUrl: './plaid-integration.component.scss'
})
export class PlaidIntegrationComponent implements OnInit {
  city: string = '';
  zipCode: string = '';

  plaidLinkToken: string | null = null;
  private plaidHandler: any;
  loading = false;
  stripe: any;
  cardElement: any;
  elements: any;
  // Stripe integration
  stripeCustomerId: string | null = null;
  paymentMethodId: string | null = null;
  stripeEmail: string = '';
  stripeName: string = '';

  constructor(private stripeService: StripeIntegrationService, private ngZone: NgZone) {
    // Cargar el script de Plaid Link dinámicamente si no está presente
    if (!(window as any).Plaid) {
      const script = document.createElement('script');
      script.src = 'https://cdn.plaid.com/link/v2/stable/link-initialize.js';
      script.async = true;
      script.onload = () => {
        // Script cargado
      };
      document.body.appendChild(script);
    }
  }

  ngOnInit(): void {
    setTimeout(() => {
      this.stripe = this.stripeService.getStripe();
      if (this.stripe) {
        this.elements = this.stripe.elements();
        this.cardElement = this.elements.create('card');
        const cardDiv = document.getElementById('card-element');
        if (cardDiv) this.cardElement.mount(cardDiv);
      }
    }, 500);
  }

  async getLinkTokenFromBackend(): Promise<string | null> {
    // Cambia la URL por la de tu backend
    const endpoint = 'http://localhost:5199/api/plaid/link-token';
    try {
      this.loading = true;
      const res = await fetch(endpoint);
      if (!res.ok) throw new Error('No se pudo obtener el link_token');
      const data = await res.json();
      return data.link_token;
    } catch (error) {
      alert('Error obteniendo link_token: ' + error);
      return null;
    } finally {
      this.loading = false;
    }
  }

  async connectWithPlaid() {
    // Esperar a que el script de Plaid esté disponible
    if (!(window as any).Plaid) {
      alert('Plaid Link aún se está cargando. Intenta de nuevo en unos segundos.');
      return;
    }
    // Obtener el link_token dinámicamente
    this.plaidLinkToken = await this.getLinkTokenFromBackend();
    if (!this.plaidLinkToken) return;

    // Inicializar el widget de Plaid Link
    this.plaidHandler = (window as any).Plaid.create({
      token: this.plaidLinkToken,
      onSuccess: async (public_token: string, metadata: any) => {
        this.loading = true;
        console.log(public_token)
        try {
          const res = await fetch('http://localhost:5199/api/plaid/exchange-public-token', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ public_token })
          });
          if (!res.ok) throw new Error('No se pudo intercambiar el public_token');
          const data = await res.json();
          alert('¡Conexión exitosa! Access token recibido.\n' + JSON.stringify(data));
        } catch (error) {
          alert('Error al intercambiar public_token: ' + error);
        } finally {
          this.loading = false;
        }
      },
      onExit: (err: any, metadata: any) => {
        if (err) {
          alert('Plaid Link salió con error: ' + err.display_message);
        }
      },
    });
    this.plaidHandler.open();
  }

  // --- Validación de cliente con Pibisi ---
  async validarClienteConPibisi() {
    // Ejemplo de datos básicos para el registro y validación (nuevo formato)
    const payload = {
      Country: 'ESP', // País del documento
      NationalId: '00000000T', // NIF válido para pruebas
      FullName: 'Juan Perez',
      BirthDate: '1980-01-01',
      City: 'Madrid',
      ZipCode: '28013'
    };
    try {
      const res = await fetch('http://localhost:5199/api/pibisi/validar-cliente', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
      });
      const data = await res.json();
      console.log('Respuesta de validación Pibisi:', data);
      alert('Resultado validación Pibisi:\n' + JSON.stringify(data, null, 2));
    } catch (error) {
      alert('Error al validar cliente en Pibisi: ' + error);
    }
  }
}
