using Microsoft.AspNetCore.Mvc;
using PlaidApi.Constants;
using PlaidApi.Plaid;
using Refit;

namespace PlaidApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaidController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly string clientId;
        private readonly string secret;
        private readonly string plaidEnv;
        public PlaidController(IConfiguration config)
        {
            _config = config;
            clientId = _config["PLAID_CLIENT_ID"];
            secret = _config["PLAID_SECRET"];
            plaidEnv = _config["PLAID_ENVIRONMENT"] ?? "sandbox";
        }
        private void VerificarCredenciales()
        {
            if (string.IsNullOrWhiteSpace(clientId))
                throw new InvalidOperationException("La variable PLAID_CLIENT_ID no está definida en los secretos de usuario ni en la configuración.");
            if (string.IsNullOrWhiteSpace(secret))
                throw new InvalidOperationException("La variable PLAID_SECRET no está definida en los secretos de usuario ni en la configuración.");
            if (string.IsNullOrWhiteSpace(plaidEnv))
                throw new InvalidOperationException("La variable PLAID_ENVIRONMENT no está definida en los secretos de usuario ni en la configuración.");
        }

        private IPlaidApi GetPlaidApi() =>
            RestService.For<IPlaidApi>($"https://{plaidEnv}.plaid.com");

        [HttpGet("link-token")]
        public async Task<IActionResult> GetLinkToken()
        {
            VerificarCredenciales();
            var plaid = GetPlaidApi();
            var request = new PlaidLinkTokenRequest
            {
                client_id = clientId,
                secret = secret,
                client_name = "Plaid Angular App",
                user = new { client_user_id = "custom_large" },
                products = ["auth", "identity", "transactions"],
                country_codes = ["ES"],
                language = "es"
            };
            var response = await plaid.CreateLinkToken(request);
            return Ok(new { link_token = response.link_token });
        }

        [HttpPost("exchange-public-token")]
        public async Task<IActionResult> ExchangePublicToken([FromBody] AccessTokenRequestDto publicToken)
        {
            VerificarCredenciales();
            var plaid = GetPlaidApi();
            var plaidExchangeTokenRequest = new PlaidExchangeTokenRequest
            {
                public_token = publicToken.AccessToken,
                client_id = clientId,
                secret = secret
            };

            var response = await plaid.ExchangePublicToken(plaidExchangeTokenRequest);

            // Verificar identidad
            var requestIdentity = new PlaidApi.Plaid.PlaidIdentityRequest
            {
                access_token = response.access_token,
                client_id = clientId,
                secret = secret
            };
            var identity = await plaid.GetIdentity(requestIdentity);

            // Obtener todas las transacciones de los últimos 90 días (con paginación)
            var endDate = System.DateTime.UtcNow.Date;
            var startDate = endDate.AddDays(-90);
            var allTransactions = new List<PlaidApi.Plaid.Transaction>();
            int offset = 0;
            int total = 0;

            do
            {
                var transactionsRequest = new PlaidApi.Plaid.PlaidTransactionsRequest
                {
                    access_token = response.access_token,
                    client_id = clientId,
                    secret = secret,
                    start_date = startDate.ToString("yyyy-MM-dd"),
                    end_date = endDate.ToString("yyyy-MM-dd"),
                    options = new Options { count = 500, offset = offset }
                };

                try
                {
                    var transactionResponse = await plaid.GetTransactions(transactionsRequest) ?? throw new Exception("Respuesta nula de la API de Plaid");
                    if (transactionResponse.transactions != null)
                    {
                        allTransactions.AddRange(transactionResponse.transactions);
                        total = transactionResponse.total_transactions;
                        offset += transactionResponse.transactions.Length;
                    }
                    else
                    {
                        throw new Exception("Lista de transacciones nula en la respuesta");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error al obtener transacciones: {ex.Message}", ex);
                }
            } while (allTransactions.Count < total);

            //promedio ingresos recurrentes
            var ingresosRecurrentes = allTransactions.Where(t =>
                     (PaymentCapacityRules.IngresosRecurrentes.Contains(t.personal_finance_category.detailed) &&
                     !PaymentCapacityRules.TransferenciasExcluir.Contains(t.personal_finance_category.detailed) &&
                     t.amount < 0) || t.amount < 0).Select(txn => Math.Abs(txn.amount)).ToList().Average();

            //promedio egresos recurrentes
            var egresosRecurrentes = allTransactions.Where(t =>
                     (PaymentCapacityRules.EgresosRecurrentes.Contains(t.personal_finance_category.detailed) &&
                     !PaymentCapacityRules.EgresosVariablesExcluir.Contains(t.personal_finance_category.detailed) &&
                     t.amount > 0) || t.amount > 0).Select(txn => txn.amount).ToList().Average();


            decimal ratio = egresosRecurrentes > 0 ? ingresosRecurrentes / egresosRecurrentes : 0;

            // Determinar nivel de capacidad
            PaymentCapacityRules.PaymentCapacityLevel nivelCapacidad = PaymentCapacityRules.GetPaymentCapacityLevel(ratio);

            // Crear resumen
            var resumen = new
            {
                ratioCapacidadPago = ratio,
                nivelCapacidad = nivelCapacidad,
                mensaje = nivelCapacidad == PaymentCapacityRules.PaymentCapacityLevel.Buena
                    ? "Felicitaciones. Préstamo otorgado. Queremos que tu experiencia sea lo más sencilla posible. Si ingresas los datos de tu tarjeta, podremos hacer el cobro automáticamente en la fecha de pago. Sin complicaciones, sin recordatorios, sin estrés."
                    : nivelCapacidad == PaymentCapacityRules.PaymentCapacityLevel.Aceptable
                    ? "Felicitaciones. Préstamo otorgado. Queremos que tu experiencia sea lo más sencilla posible. Si ingresas los datos de tu tarjeta, podremos hacer el cobro automáticamente en la fecha de pago. Sin complicaciones, sin recordatorios, sin estrés."
                    : "ALERTAS: No hemos podido avanzar con tu solicitud debido a criterios internos relacionados con tu capacidad de pago."
            };

            return Ok(new { plaid = response, resumen, transacciones = allTransactions });
        }
    }
}
