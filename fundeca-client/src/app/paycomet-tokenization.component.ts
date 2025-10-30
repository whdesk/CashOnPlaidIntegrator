import { Component, OnInit, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-paycomet-tokenization',
  templateUrl: './paycomet-tokenization.component.html',
  styleUrls: ['./paycomet-tokenization.component.scss'],
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule]
})
export class PaycometTokenizationComponent implements OnInit, AfterViewInit {
  loading = false;
  error: string | null = null;
  idUser: string | null = null;
  tokenUser: string | null = null;
  amount = 100; // céntimos
  currency = 'EUR';
  orderId = '';
  jetPublicKey: string | null = null;
  jetReady = false;
  jetIdInDom: string | null = null;
  // Debug
  panIframeReady = false;
  cvcIframeReady = false;
  jetTokenDebug: string | null = null;
  cardForm!: FormGroup;

  // TODO: mover a environment para dev. Usar header x-api-key para backend
  private apiBase = 'http://localhost:5199/api/paycomet';
  private apiKey = 'dev-api-key';

  constructor(private cd: ChangeDetectorRef) {}

  async ngOnInit(): Promise<void> {
    // Inicializar Reactive Form para los campos no sensibles
    this.cardForm = new FormBuilder().group({
      holder: ['', [Validators.required, Validators.minLength(2)]],
      month: ['', [Validators.required, Validators.pattern(/^(0[1-9]|1[0-2])$/)]],
      year: ['', [Validators.required, Validators.pattern(/^\d{2}$/)]]
    });
  }

  async ngAfterViewInit(): Promise<void> {
    // Defer para evitar NG0100 al cambiar estados durante el mismo ciclo de verificación
    setTimeout(() => this.loadConfigAndSdk(), 0);
  }

  private async addCardWithToken(jetToken: string) {
    try {
      this.loading = true;
      console.log('[paycomet] addCardWithToken: token presente');
      this.jetTokenDebug = jetToken;
      const res = await fetch(`${this.apiBase}/add-card`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ jetToken })
      });
      const data = await res.json();
      console.log('[paycomet] add-card respuesta', res.status, data);
      if (!res.ok) {
        this.error = data?.error || JSON.stringify(data);
        return;
      }
      this.idUser = data?.idUser || null;
      this.tokenUser = data?.tokenUser || null;
    } catch (e: any) {
      this.error = e?.message || 'Error en addCard';
    } finally {
      this.loading = false;
    }
  }

  async onSubmit(event: Event) {
    try {
      console.log('[paycomet] onSubmit');
      // Garantizar que el jetID está como atributo y valor antes de que el SDK procese el submit
      this.ensureJetIdAttribute();
      this.error = null;
      this.loading = true;
      // Fallback: si el SDK no invocó el callback tras 1s, intentamos leer el hidden
      setTimeout(async () => {
        if (!this.loading) return; // ya finalizó por callback
        console.log('[paycomet] fallback: intento de lectura de hidden jetToken');
        const tokenInput: HTMLInputElement | null = document.querySelector('input[name="paytpvToken"], input[name="jetToken"]');
        const jetToken = tokenInput?.value;
        if (jetToken) {
          await this.addCardWithToken(jetToken);
        } else {
          this.loading = false;
          this.error = 'No se generó el jetToken. Verifica que el script JET cargó y que los campos (titular/fecha/PAN/CVC) son válidos.';
        }
      }, 1000);
    } catch (e: any) {
      this.error = e?.message || 'Error procesando el formulario JET';
    } finally {
    }
  }

  private async loadConfigAndSdk() {
    try {
      // Defer del cambio de estado para evitar NG0100
      await new Promise((r) => setTimeout(r, 0));
      this.loading = true;
      const res = await fetch(`${this.apiBase}/config`);
      const data = await res.json();
      if (!res.ok) {
        this.error = data?.error || 'No se pudo obtener configuración PayCOMET';
        return;
      }
      this.jetPublicKey = data?.jetPublicKey || null;
      if (!data?.hasJetKey) {
        this.error = 'Falta PAYCOMET_JET_PUBLIC_KEY en el backend. Añádelo en appsettings.json.';
        return;
      }
      // Esperar al siguiente tick para asegurar que Angular pintó el DOM
      await new Promise((r) => setTimeout(r, 0));
      // Garantizar que el input hidden tenga también el atributo value (no sólo la propiedad)
      this.ensureJetIdAttribute();
      // Registrar callback global antes de cargar el SDK
      (window as any).paycometHandler = async (resp: any) => {
        console.log('[paycometHandler] respuesta SDK:', resp);
        try {
          const tokenFromResp = resp?.paytpvToken || resp?.jetToken;
          if (tokenFromResp) {
            this.jetTokenDebug = tokenFromResp;
            await this.addCardWithToken(tokenFromResp);
            return;
          }
          // fallback: leer hidden
          const tokenInput: HTMLInputElement | null = document.querySelector('input[name="paytpvToken"], input[name="jetToken"]');
          const jetToken = tokenInput?.value;
          if (jetToken) {
            this.jetTokenDebug = jetToken;
            await this.addCardWithToken(jetToken);
          } else {
            this.error = 'Callback JET recibido pero no se localizó jetToken';
          }
        } catch (e: any) {
          this.error = e?.message || 'Error en callback de JET';
        } finally {
          this.loading = false;
        }
      };
      // Cargar el SDK JET inicialmente (no sólo en fallback)
      await this.injectScript('https://api.paycomet.com/gateway/paycomet.jetiframe.js');
      // Ahora sí, marcar listo tras la carga del script
      this.jetReady = true;
      // Leer jetID real presente en el DOM para diagnóstico
      this.probeJetId();
      // Marcar si los iframes han sido inyectados
      const rescan = () => {
        this.panIframeReady = !!document.querySelector('#paycomet-pan iframe');
        this.cvcIframeReady = !!document.querySelector('#paycomet-cvc2 iframe');
      };
      rescan();
      setTimeout(rescan, 300);
      setTimeout(rescan, 1000);
      // Fallback: si tras 1200ms los iframes no existen, reinyectar el SDK con cache-busting
      setTimeout(async () => {
        if (!this.panIframeReady || !this.cvcIframeReady) {
          console.warn('[paycomet] iframes no cargaron con script estático, reinyectando SDK...');
          try {
            await this.injectScript(`https://api.paycomet.com/gateway/paycomet.jetiframe.js?retry=${Date.now()}`);
            // Re-escanear tras breve espera
            setTimeout(rescan, 300);
            setTimeout(rescan, 1000);
          } catch (e) {
            console.error('[paycomet] error reinyectando SDK', e);
          }
        }
      }, 1200);
      // Observar inserción de iframes
      try {
        const pan = document.getElementById('paycomet-pan');
        const cvc = document.getElementById('paycomet-cvc2');
        const obs = new MutationObserver(() => rescan());
        if (pan) obs.observe(pan, { childList: true, subtree: true });
        if (cvc) obs.observe(cvc, { childList: true, subtree: true });
      } catch { /* noop */ }
      // Si el script aún no cargó, el callback se registró igualmente y se llamará al enviar
    } catch (e: any) {
      this.error = (e && e.message) ? `Error cargando SDK JET: ${e.message}` : 'Error cargando SDK JET';
    } finally {
      this.loading = false;
      // Evitar ExpressionChangedAfterItHasBeenCheckedError por cambios async en after view init
      Promise.resolve().then(() => this.cd.detectChanges());
    }
  }

  private injectScript(src: string): Promise<void> {
    return new Promise((resolve, reject) => {
      const existing = document.querySelector(`script[src="${src}"]`);
      if (existing) return resolve();
      const s = document.createElement('script');
      s.src = src;
      s.async = true;
      s.onload = () => resolve();
      s.onerror = () => reject(new Error('No se pudo cargar el SDK JET'));
      document.head.appendChild(s);
    });
  }

  async addCard() {
    try {
      this.loading = true;
      this.error = null;
      console.log('[paycomet] addCard fallback leyendo jetToken');
      const tokenInput: HTMLInputElement | null = document.querySelector('input[name="paytpvToken"], input[name="jetToken"]');
      const jetToken = tokenInput?.value;
      console.log('[paycomet] jetToken hidden:', !!jetToken);
      if (!jetToken) {
        this.error = 'No se encontró jetToken generado por JET. Asegúrate de que el script cargó y el formulario fue enviado.';
        return;
      }
      const res = await fetch(`${this.apiBase}/add-card`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ jetToken })
      });
      const data = await res.json();
      console.log('[paycomet] add-card respuesta (fallback)', res.status, data);
      if (!res.ok) {
        this.error = data?.error || JSON.stringify(data);
        return;
      }
      this.idUser = data?.idUser || null;
      this.tokenUser = data?.tokenUser || null;
    } catch (e: any) {
      this.error = e?.message || 'Error en addCard';
    } finally {
      this.loading = false;
    }
  }

  async chargeToken() {
    try {
      this.loading = true;
      this.error = null;
      if (!this.idUser || !this.tokenUser) {
        this.error = 'Falta idUser/tokenUser. Guarda la tarjeta primero.';
        return;
      }
      const res = await fetch(`${this.apiBase}/charge-token`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', 'x-api-key': this.apiKey },
        body: JSON.stringify({ idUser: this.idUser, tokenUser: this.tokenUser, amount: this.amount, currency: this.currency, orderId: this.orderId || undefined, mit: true })
      });
      const data = await res.json();
      if (!res.ok) {
        this.error = data?.error || JSON.stringify(data);
        return;
      }
      alert(`Estado: ${data?.status || 'OK'} - Importe: ${data?.amount} ${data?.currency}`);
    } catch (e: any) {
      this.error = e?.message || 'Error al cobrar con token';
    } finally {
      this.loading = false;
    }
  }

  private probeJetId() {
    try {
      const el = document.querySelector('input[data-paycomet="jetID"]') as HTMLInputElement | null;
      const val = el?.value?.trim();
      this.jetIdInDom = val && val.length > 0 ? val : null;
      console.log('[paycomet] jetID en DOM:', this.jetIdInDom);
    } catch { /* noop */ }
  }

  private ensureJetIdAttribute() {
    try {
      const el = document.querySelector('input[data-paycomet="jetID"]') as HTMLInputElement | null;
      if (!el) return;
      const desired = (this.jetPublicKey || el.value || '').trim();
      if (!desired) return;
      // Establecer tanto la propiedad como el atributo HTML para compatibilidad
      el.value = desired;
      el.setAttribute('value', desired);
      console.log('[paycomet] ensureJetIdAttribute -> value attr:', el.getAttribute('value'));
    } catch { /* noop */ }
  }
}
