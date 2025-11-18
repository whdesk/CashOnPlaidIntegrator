import React, { useCallback, useEffect, useRef, useState } from 'react'

const API_BASE = (import.meta as any).env?.VITE_TESTER_API_URL || 'http://localhost:5298'
const FUC = (import.meta as any).env?.VITE_REDSYS_FUC || ''
const TERMINAL = (import.meta as any).env?.VITE_REDSYS_TERMINAL || ''
const INSITE_LANG = (import.meta as any).env?.VITE_REDSYS_INSITE_LANG || 'ES'

function loadRedsysScript(): Promise<void> {
  return new Promise((resolve, reject) => {
    const existing = document.querySelector('script[data-redsys="v3"]') as HTMLScriptElement | null
    if (existing) {
      resolve()
      return
    }
    const s = document.createElement('script')
    s.src = 'https://sis-t.redsys.es:25443/sis/NC/sandbox/redsysV3.js'
    s.async = true
    s.defer = true
    s.dataset.redsys = 'v3'
    s.onload = () => resolve()
    s.onerror = () => reject(new Error('No se pudo cargar redsysV3.js'))
    document.head.appendChild(s)
  })
}

async function waitForInSiteApi(maxMs = 7000, intervalMs = 200): Promise<void> {
  const start = Date.now()
  return new Promise((resolve, reject) => {
    const tick = () => {
      // @ts-expect-error redsys global
      const ok = typeof window.getInSiteForm === 'function' && typeof window.storeIdOper === 'function'
      if (ok) return resolve()
      if (Date.now() - start >= maxMs) return reject(new Error('inSite API no disponible a tiempo'))
      setTimeout(tick, intervalMs)
    }
    tick()
  })
}

function generateOrderD12() {
  const n = Math.floor(Math.random() * 999999999999)
  return n.toString().padStart(12, '0')
}

export default function RedsysInSiteUnified() {
  const [ready, setReady] = useState(false)
  const [order, setOrder] = useState<string>(generateOrderD12())
  const [idOper, setIdOper] = useState<string>('')
  const [errorCode, setErrorCode] = useState<string>('')
  const [amount, setAmount] = useState<number>(100) // cents
  const [loading, setLoading] = useState(false)
  const [uiError, setUiError] = useState<string>('')
  const formContainerRef = useRef<HTMLDivElement>(null)

  // Cargar script y pintar inSite unificado
  useEffect(() => {
    let unsub: (() => void) | null = null

    const boot = async () => {
      await loadRedsysScript()
      try {
        await waitForInSiteApi(7000, 200)
      } catch (e) {
        console.warn('Funciones inSite no disponibles aún')
      }

      // Listener para recibir idOper
      const tokenInput = document.getElementById('insite-token') as HTMLInputElement
      const errInput = document.getElementById('insite-error') as HTMLInputElement
      const receiveMessage = (event: MessageEvent) => {
        try {
          // @ts-expect-error storeIdOper global por script
          window.storeIdOper(event, 'insite-token', 'insite-error', () => true)
          setIdOper(tokenInput?.value || '')
          setErrorCode(errInput?.value || '')
        } catch (e) {
          console.error('storeIdOper error', e)
        }
      }
      window.addEventListener('message', receiveMessage, false)
      unsub = () => window.removeEventListener('message', receiveMessage, false)

      // Evitar repintar si ya quedó listo
      if (ready) return

      // Limpia posibles restos en el contenedor
      const container = document.getElementById('insite-card-form') as HTMLDivElement | null
      if (container) container.innerHTML = ''

      // Pintar formulario unificado
      try {
        if (!FUC || !TERMINAL) {
          setUiError('Configura VITE_REDSYS_FUC y VITE_REDSYS_TERMINAL en .env y reinicia el dev server')
          return
        }

        // @ts-expect-error global
        if (typeof window.getInSiteForm === 'function') {
          // @ts-expect-error global
          window.getInSiteForm(
            'insite-card-form',
            '',
            '',
            '',
            '',
            'Pagar con Redsys',
            FUC,
            TERMINAL,
            order,
            INSITE_LANG,
            true,
            false,
            'twoRows'
          )
          setReady(true)
        } else if (typeof (window as any).getInSiteFormJSON === 'function') {
          ;(window as any).getInSiteFormJSON({
            id: 'insite-card-form',
            fuc: FUC,
            terminal: TERMINAL,
            order,
            idiomaInsite: INSITE_LANG,
            estiloInsite: 'inline',
            mostrarLogo: true,
          })
          setReady(true)
        } else {
          setUiError('API inSite no disponible (getInSiteForm / getInSiteFormJSON)')
        }
      } catch (e) {
        console.error('getInSiteForm error', e)
        setUiError('No se pudo pintar el iframe inSite (revisa consola y variables .env)')
      }
    }

    boot()
    return () => {
      if (unsub) unsub()
    }
  }, [order, ready])

  const handleAuthorize = useCallback(async () => {
    if (!idOper) return
    setLoading(true)
    try {
      const resp = await fetch(`${API_BASE}/redsys/insite/authorize`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ IdOper: idOper, Order: order, Amount: amount })
      })
      if (!resp.ok) throw new Error(`HTTP ${resp.status}`)
      const data = await resp.json()

      const form = document.createElement('form')
      form.method = 'POST'
      form.action = data.formUrl || data.FormUrl
      form.style.display = 'none'

      const i1 = document.createElement('input')
      i1.type = 'hidden'
      i1.name = 'Ds_SignatureVersion'
      i1.value = data.Ds_SignatureVersion || data.ds_SignatureVersion

      const i2 = document.createElement('input')
      i2.type = 'hidden'
      i2.name = 'Ds_MerchantParameters'
      i2.value = data.Ds_MerchantParameters || data.ds_MerchantParameters

      const i3 = document.createElement('input')
      i3.type = 'hidden'
      i3.name = 'Ds_Signature'
      i3.value = data.Ds_Signature || data.ds_Signature

      form.appendChild(i1)
      form.appendChild(i2)
      form.appendChild(i3)
      document.body.appendChild(form)
      form.submit()
      document.body.removeChild(form)
    } catch (e) {
      console.error('Authorize error', e)
      alert('Error al autorizar inSite')
    } finally {
      setLoading(false)
    }
  }, [idOper, order, amount])

  return (
    <div className="card" style={{ marginTop: 24 }}>
      <h3>inSite (Unificado) → Obtener idOper y autorizar</h3>

      <div style={{ marginBottom: 8 }}>
        <small>FUC y TERMINAL se leen de variables: VITE_REDSYS_FUC / VITE_REDSYS_TERMINAL</small>
      </div>

      <div style={{ display: 'flex', gap: 12, alignItems: 'center', flexWrap: 'wrap' }}>
        <div>
          <label>Order (D12)</label>
          <input
            value={order}
            onChange={(e) => {
              const next = e.target.value.replace(/\D/g, '').slice(0, 12).padStart(12, '0')
              setOrder(next)
              setIdOper('')
              setErrorCode('')
              setReady(false)
            }}
            style={{ marginLeft: 8 }}
          />
          <button
            style={{ marginLeft: 8 }}
            onClick={() => {
              setOrder(generateOrderD12())
              setIdOper('')
              setErrorCode('')
              setReady(false)
            }}
          >
            Nuevo
          </button>
        </div>
        <div>
          <label>Importe (€)</label>
          <input
            type="number"
            min={0.01}
            step={0.01}
            value={amount / 100}
            onChange={(e) => setAmount(Math.round(Number(e.target.value) * 100))}
            style={{ marginLeft: 8 }}
          />
        </div>
      </div>

      <div
        id="insite-card-form"
        ref={formContainerRef}
        style={{
          marginTop: 12,
          minHeight: 420,
          display: 'grid',
          border: '1px solid #e5e7eb',
          borderRadius: 6,
          padding: 8,
          backgroundColor: '#ffffff',
          color: '#111827'
        }}
      />

      {uiError && (
        <div style={{ marginTop: 8, color: 'orangered' }}>
          {uiError}
        </div>
      )}

      <input id="insite-token" type="hidden" />
      <input id="insite-error" type="hidden" />

      <div style={{ marginTop: 12 }}>
        <div>idOper: <code>{idOper || '-'}</code></div>
        {errorCode && <div style={{ color: 'orangered' }}>Error inSite: {errorCode}</div>}
      </div>

      <div style={{ marginTop: 12 }}>
        <button className="btn" disabled={!idOper || loading} onClick={handleAuthorize}>
          {loading ? 'Autorizando…' : 'Autorizar con idOper'}
        </button>
      </div>
    </div>
  )
}
