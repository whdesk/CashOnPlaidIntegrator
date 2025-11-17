import React, { useEffect, useRef, useState } from 'react'
import { useRedsysTokenize, TokenizeStartResponse } from '../hooks/useRedsysTokenize'
import { useVerifyRedsys } from '../hooks/useVerifyRedsys'
import { useRedsysMit } from '../hooks/useRedsysMit';

export default function RedsysAddCardWithVerify() {
  const { start, loading, error } = useRedsysTokenize()
  const { checking, setChecking, done, identifier, raw } = useVerifyRedsys()
  const { start: startMitCharge, loading: mitLoading, error: mitError, queryTransaction } = useRedsysMit();

  const [payload, setPayload] = useState<Pick<TokenizeStartResponse, 'formUrl' | 'ds_SignatureVersion' | 'ds_MerchantParameters' | 'ds_Signature'> | null>(null)
  const formRef = useRef<HTMLFormElement>(null)

  const [amount, setAmount] = useState(100); // Monto en céntimos
  const [lastOrder, setLastOrder] = useState('');
  const [queryResult, setQueryResult] = useState<any>(null);

  const onClick = async () => {
    try {
      const res = await start()
      if (res) {
        if (!res.ds_Signature || !res.ds_MerchantParameters) {
          throw new Error('Firma o parámetros incompletos desde el servidor')
        }
        setPayload({
          formUrl: res.formUrl,
          ds_SignatureVersion: res.ds_SignatureVersion,
          ds_MerchantParameters: res.ds_MerchantParameters,
          ds_Signature: res.ds_Signature,
        })
      }
    } catch (err: unknown) {
      console.error('Error en tokenización:', err)
      const errorMsg = err instanceof Error ? err.message : 'Error desconocido'
      if (errorMsg.includes('firma')) {
        alert('Error en la firma digital. Verifica la clave secreta configurada.')
      }
    }
  }

  useEffect(() => {
    if (payload && formRef.current) {
      setChecking(true)
      formRef.current.submit()
    }
  }, [payload, setChecking])

  const handleMitCharge = async () => {
    if (!identifier) return;
    
    try {
      const res = await startMitCharge(identifier, amount);
      if (res?.Ds_Signature) {
        const form = document.createElement('form');
        form.method = 'POST';
        form.action = res.FormUrl;
        form.innerHTML = `
          <input type="hidden" name="Ds_SignatureVersion" value="${res.Ds_SignatureVersion}">
          <input type="hidden" name="Ds_MerchantParameters" value="${res.Ds_MerchantParameters}">
          <input type="hidden" name="Ds_Signature" value="${res.Ds_Signature}">
        `;
        document.body.appendChild(form);
        form.submit();
      }
    } catch (err) {
      console.error('Error en cargo MIT:', err);
    }
  };

  const handleQuery = async () => {
    if (!lastOrder) return;
    try {
      const result = await queryTransaction(lastOrder);
      setQueryResult(result);
    } catch (err) {
      console.error('Error consultando:', err);
    }
  };

  return (
    <div className="card">
      <div className="row">
        <button className="btn" onClick={onClick} disabled={loading || checking}>
          {loading ? 'Preparando…' : checking ? 'Verificando…' : 'Añadir tarjeta (Redsys)'}
        </button>
        {error && <span className="error">{error}</span>}
      </div>

      {done && (
        <div className="success">
          <div>Tarjeta tokenizada correctamente (MIT)</div>
          <div>Identifier: <code>{identifier}</code></div>
          <div style={{ marginTop: 8 }}>
            <small>Guarda este identificador para cargos futuros</small>
          </div>
        </div>
      )}

      {/* Depuración opcional */}
      {raw && (
        <details className="debug">
          <summary>Detalles</summary>
          <pre>{JSON.stringify(raw, null, 2)}</pre>
        </details>
      )}

      {payload && (
        <form
          ref={formRef}
          method="POST"
          action={payload.formUrl}
          target="_self"
          style={{ display: 'none' }}
        >
          <input type="hidden" name="Ds_SignatureVersion" value={payload.ds_SignatureVersion} />
          <input type="hidden" name="Ds_MerchantParameters" value={payload.ds_MerchantParameters} />
          <input type="hidden" name="Ds_Signature" value={payload.ds_Signature} />
        </form>
      )}

      {identifier && (
        <div className="mit-section" style={{ marginTop: '1rem' }}>
          <h4>Cargo MIT</h4>
          <input 
            type="number" 
            value={amount / 100} 
            onChange={(e) => setAmount(Math.round(Number(e.target.value) * 100))}
            min="0.01"
            step="0.01"
          />
          <button
            onClick={handleMitCharge}
            disabled={mitLoading}
            className="btn btn-primary"
          >
            {mitLoading ? 'Procesando...' : 'Realizar Cargo'}
          </button>
          {mitError && <div className="error">{mitError}</div>}
        </div>
      )}

      <div className="query-section">
        <input 
          value={lastOrder}
          onChange={(e) => setLastOrder(e.target.value)}
          placeholder="Número de orden"
        />
        <button 
          onClick={handleQuery}
          className="btn btn-info"
        >
          Consultar Transacción
        </button>
        {queryResult && (
          <div className="query-result">
            <pre>{JSON.stringify(queryResult, null, 2)}</pre>
          </div>
        )}
      </div>
    </div>
  )
}
