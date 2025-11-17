import { useState } from 'react';

const API_BASE = (import.meta as any).env?.VITE_TESTER_API_URL || 'http://localhost:5298'

export type MitChargeResponse = {
  Ds_SignatureVersion: string;
  Ds_MerchantParameters: string;
  Ds_Signature: string;
  Order: string;
  FormUrl: string;
};

export type QueryResponse = any; // Agregar tipo de respuesta para la consulta

export const useRedsysMit = (apiBase: string = API_BASE) => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const start = async (
    identifier: string, 
    amountCents: number,
    order?: string
  ): Promise<MitChargeResponse | null> => {
    try {
      setLoading(true);
      setError(null);
      
      const res = await fetch(`${apiBase}/redsys/mit/charge`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ 
          identifier, 
          amount: amountCents,
          order
        })
      });

      if (!res.ok) {
        const errorData = await res.json();
        throw new Error(errorData.message || 'Error en cargo MIT');
      }

      return await res.json();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Error desconocido');
      return null;
    } finally {
      setLoading(false);
    }
  };

  const queryTransaction = async (order: string): Promise<any> => {
    try {
      const res = await fetch(`/redsys/query?order=${order}`);
      if (!res.ok) throw new Error('Error en consulta');
      return await res.json();
    } catch (err) {
      console.error('Error:', err);
      throw err;
    }
  };

  return { 
    start, 
    queryTransaction,
    loading, 
    error,
    resetError: () => setError(null)
  };
};
