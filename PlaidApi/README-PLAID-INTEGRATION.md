# PlaidApi (ASP.NET Core)

## Endpoints creados

- `GET /api/plaid/link-token`: Genera un link_token para inicializar Plaid Link en el frontend.
- `POST /api/plaid/exchange-public-token`: Intercambia un public_token por un access_token.

## Configuración
1. Cambia los valores de `clientId`, `secret` y `plaidEnv` en `PlaidController.cs` por tus credenciales reales de Plaid.
2. El endpoint `/api/plaid/link-token` debe devolver `{ link_token: "..." }`.
3. El endpoint `/api/plaid/exchange-public-token` espera `{ "public_token": "..." }` y devuelve la respuesta de Plaid.
4. El CORS está habilitado para permitir peticiones desde el frontend Angular.

## Ejecución

```
dotnet run --project PlaidApi/PlaidApi.csproj
```

La API estará disponible en http://localhost:5199

## Seguridad
- ¡No expongas tus credenciales en el frontend!
- Usa variables de entorno o `IConfiguration` para producción.

---

Tu frontend Angular debe apuntar a:
- `http://localhost:5199/api/plaid/link-token`
- `http://localhost:5199/api/plaid/exchange-public-token`

Ajusta las URLs en el frontend si cambias el puerto/base de la API.
