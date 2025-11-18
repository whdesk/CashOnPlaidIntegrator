import React, { useState } from 'react'

const API_BASE = (import.meta as any).env?.VITE_TESTER_API_URL || 'http://localhost:5298'

function generateOrderD12() {
  const n = Math.floor(Math.random() * 999999999999)
  return n.toString().padStart(12, '0')
}

export default function RedsysBizumRedirect() {
  const [order, setOrder] = useState<string>(generateOrderD12())
  const [amount, setAmount] = useState<number>(100)
  const [phone, setPhone] = useState<string>('')
  const [loading, setLoading] = useState(false)

  const handleBizumPay = async () => {
    if (!phone) {
      alert('Introduce un teléfono Bizum con prefijo, por ejemplo +346XXXXXXXX')
      return
    }
    if (amount <= 0) {
      alert('Importe inválido')
      return
    }
    setLoading(true)
    try {
      const resp = await fetch(`${API_BASE}/redsys/bizum/start`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ amount, order, phone })
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
      console.error('Bizum start error', e)
      alert('Error al iniciar pago Bizum')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="card" style={{ marginTop: 24 }}>
      <h3>Pago Bizum (Redirección)</h3>
      <p>Genera orden y parámetros para pago Bizum via TPV Virtual.</p>

      <div style={{ display: 'flex', flexDirection: 'column', gap: 8 }}>
        <div>
          <label>Order (D12)</label>
          <input
            value={order}
            onChange={(e) => setOrder(e.target.value.replace(/\D/g, '').slice(0, 12).padStart(12, '0'))}
            style={{ marginLeft: 8 }}
          />
          <button style={{ marginLeft: 8 }} onClick={() => setOrder(generateOrderD12())}>Nuevo</button>
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

        <div>
          <label>Teléfono Bizum (con prefijo)</label>
          <input
            type="text"
            value={phone}
            onChange={(e) => setPhone(e.target.value)}
            placeholder="+346XXXXXXXX"
            style={{ marginLeft: 8, minWidth: 180 }}
          />
        </div>
      </div>

      <div style={{ marginTop: 12 }}>
        <button className="btn" onClick={handleBizumPay} disabled={loading}>
          {loading ? 'Redirigiendo…' : 'Pagar con Bizum'}
        </button>
      </div>
    </div>
  )
}
