RedsysTesterFrontend

Comandos:
- npm install
- npm run dev (http://localhost:5174)

Config:
- El frontend asume el backend tester en http://localhost:5298
- Puedes cambiar la URL con la variable VITE_TESTER_API_URL (no versionada por .gitignore)

Estructura:
- src/App.tsx: App básica con el flujo de tokenización
- src/components/RedsysAddCardWithVerify.tsx: Componente con auto-post al SIS y polling
- src/hooks/useRedsysTokenize.ts: Llama a /redsys/tokenize/start
- src/hooks/useVerifyRedsys.ts: Polling contra /redsys/last-status

Notas:
- Para .env, crea manualmente un archivo .env.local con VITE_TESTER_API_URL=http://localhost:5298
- Los archivos .env están ignorados por el repo; por eso no se incluyen aquí.
