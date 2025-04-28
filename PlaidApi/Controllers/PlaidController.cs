using Microsoft.AspNetCore.Mvc;
using PlaidApi.Plaid;
using Refit;

namespace PlaidApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaidController : ControllerBase
    {
        // TODO: Coloca tus credenciales reales aquí o usa IConfiguration para mayor seguridad
        private readonly string clientId = "631b6a91e93a3f001414f109";
        private readonly string secret = "679f5277a2b1806423fc309372ca8e";
        private readonly string plaidEnv = "sandbox"; // o "development" o "production"

        private IPlaidApi GetPlaidApi() =>
            RestService.For<IPlaidApi>($"https://{plaidEnv}.plaid.com");

        [HttpGet("link-token")]
        public async Task<IActionResult> GetLinkToken()
        {
            var plaid = GetPlaidApi();
            var request = new PlaidLinkTokenRequest
            {
                client_id = clientId,
                secret = secret,
                client_name = "Plaid Angular App",
                user = new { client_user_id = "user-123" },
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
            var plaid = GetPlaidApi();
            var plaidExchangeTokenRequest = new PlaidExchangeTokenRequest
            {
                public_token = publicToken.AccessToken,
                client_id = clientId,
                secret = secret
            };

            var response = await plaid.ExchangePublicToken(plaidExchangeTokenRequest);

            var auth = new PlaidApi.Plaid.PlaidAuthRequest
            {
                access_token = response.access_token,
                client_id = clientId,
                secret = secret
            };

            var authResponse = await plaid.GetAuth(auth);

            var requestIdentity = new PlaidApi.Plaid.PlaidIdentityRequest
            {
                access_token = response.access_token,
                client_id = clientId,
                secret = secret
            };

            var identity = await plaid.GetIdentity(requestIdentity);


            // Calcular fechas de los últimos 3 meses
            var endDate = System.DateTime.UtcNow.Date;
            var startDate = endDate.AddMonths(-3);

            // Paginación para obtener todas las transacciones de los últimos 3 meses
            var allTransactions = new List<PlaidApi.Plaid.Transaction>();
            int totalTransactions = 0;
            int count = 500; // máximo permitido por Plaid
            int offset = 0;
            PlaidApi.Plaid.PlaidTransactionsResponse transactionResponse = null;

            var transactions2 = new PlaidApi.Plaid.PlaidTransactionsRequest
            {
                access_token = response.access_token,
                client_id = clientId,
                secret = secret,
                start_date = startDate.ToString("yyyy-MM-dd"),
                end_date = endDate.ToString("yyyy-MM-dd"),
                options = new PlaidApi.Plaid.Options { count = count, offset = offset }
            };

            var transactionResponse2= await plaid.GetTransactions2(transactions2);
            try
            {
                while (true)
                {
                    var transactions = new PlaidApi.Plaid.PlaidTransactionsRequest
                    {
                        access_token = response.access_token,
                        client_id = clientId,
                        secret = secret,
                        start_date = startDate.ToString("yyyy-MM-dd"),
                        end_date = endDate.ToString("yyyy-MM-dd"),
                        options = new PlaidApi.Plaid.Options { count = count, offset = offset }
                    };

                    transactionResponse = await plaid.GetTransactions(transactions);
                    if (transactionResponse?.transactions != null)
                        allTransactions.AddRange(transactionResponse.transactions);

                    totalTransactions = transactionResponse?.total_transactions ?? 0;
                    offset += count;

                    if (allTransactions.Count >= totalTransactions || transactionResponse?.transactions == null || transactionResponse.transactions.Length == 0)
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener transacciones: {ex.Message}");

            }


            // Procesar ingresos y egresos de forma robusta usando categorías
            var ingresos = 0.0M;
            var egresos = 0.0M;
            if (allTransactions != null)
            {
                // Categorías típicas de ingreso y egreso
                var categoriasIngreso = new[] { "Deposit", "Payroll", "Refund", "Interest" };
                var categoriasEgreso = new[] { "Payment", "Purchase", "Transfer", "Withdrawal" };

                foreach (var txn in allTransactions)
                {
                    bool isIngreso = false;
                    bool isEgreso = false;
                    var catArray = txn.category ?? [];
                    var catString = string.Join(" ", catArray);

                    if (txn.amount < 0)
                        isIngreso = true;
                    else if (txn.amount > 0)
                    {
                        // Si la categoría contiene alguna palabra clave de ingreso
                        if (categoriasIngreso.Any(ci => catString.Contains(ci, System.StringComparison.OrdinalIgnoreCase)))
                            isIngreso = true;
                        // Si la categoría contiene alguna palabra clave de egreso
                        else if (categoriasEgreso.Any(ce => catString.Contains(ce, System.StringComparison.OrdinalIgnoreCase)))
                            isEgreso = true;
                        else
                            isEgreso = true; // Por defecto, positivos son egreso
                    }

                    if (isIngreso)
                        ingresos += System.Math.Abs(txn.amount);
                    if (isEgreso)
                        egresos += System.Math.Abs(txn.amount);
                }
            }

            var resumen = new
            {
                ingresos,
                egresos,
                mas_egresos_que_ingresos = egresos > ingresos
            };

            return Ok(new { plaid = response, resumen, transacciones = allTransactions });
        }
    }

    public class PublicTokenRequest
    {
        public string public_token { get; set; }
    }


}
