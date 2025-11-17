import { useState } from 'react'

const API_BASE = (import.meta as any).env?.VITE_TESTER_API_URL || 'http://localhost:5298'

export type TokenizeStartResponse = {
  formUrl: string
  ds_SignatureVersion: string
  ds_MerchantParameters: string
  ds_Signature: string
  order: string
}

export function useRedsysTokenize(apiBase: string = API_BASE) {
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)

  const start = async (): Promise<TokenizeStartResponse | null> => {
    try {
      setLoading(true)
      setError(null)
      const res = await fetch(`${apiBase}/redsys/tokenize/start`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
      })
      if (!res.ok) throw new Error(`HTTP ${res.status}`)
      return (await res.json()) as TokenizeStartResponse
    } catch (e: any) {
      setError(e?.message ?? 'Error inesperado')
      return null
    } finally {
      setLoading(false)
    }
  }

  return { start, loading, error }
}
