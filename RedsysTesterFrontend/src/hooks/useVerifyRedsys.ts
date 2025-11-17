import { useEffect, useState } from 'react'

const API_BASE = (import.meta as any).env?.VITE_TESTER_API_URL || 'http://localhost:5298'

export function useVerifyRedsys(apiBase: string = API_BASE) {
  const [checking, setChecking] = useState(false)
  const [done, setDone] = useState(false)
  const [identifier, setIdentifier] = useState<string | null>(null)
  const [raw, setRaw] = useState<any>(null)

  useEffect(() => {
    if (!checking) return
    const id = setInterval(async () => {
      try {
        const res = await fetch(`${apiBase}/redsys/last-status`)
        const data = await res.json()
        if (data?.completed) {
          setIdentifier(data.identifier ?? null)
          setRaw(data)
          setDone(true)
          setChecking(false)
        }
      } catch {
        // ignore polling errors
      }
    }, 2000)
    return () => clearInterval(id)
  }, [checking, apiBase])

  return { checking, setChecking, done, identifier, raw }
}
