using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace PlaidApi.Controllers
{
    [ApiController]
    [Route("api/paycomet")]
    public class PaycometController : ControllerBase
    {
        private readonly IConfiguration _config;

        public PaycometController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("config")]
        public IActionResult GetConfig()
        {
            var jet = _config["PAYCOMET_JET_PUBLIC_KEY"];
            return Ok(new { jetPublicKey = jet, hasJetKey = !string.IsNullOrWhiteSpace(jet) });
        }

        [HttpPost("add-card")]
        public async Task<IActionResult> AddCard([FromBody] AddCardRequest request)
        {
            if (!IsAuthorized()) return Unauthorized(new { error = "invalid api key" });
            var terminal = _config["PAYCOMET_TERMINAL"];
            var apiKey = _config["PAYCOMET_API_KEY"];
            var jetKey = _config["PAYCOMET_JET_PUBLIC_KEY"];
            if (string.IsNullOrWhiteSpace(terminal) || string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(jetKey))
            {
                return BadRequest(new { error = "Faltan credenciales PAYCOMET (terminal/apiKey/jetPublicKey)" });
            }
            if (string.IsNullOrWhiteSpace(request?.JetToken))
            {
                return BadRequest(new { error = "jetToken es requerido" });
            }

            using var http = new HttpClient { BaseAddress = new Uri("https://api.paycomet.com/") };
            http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
            var payload = new
            {
                terminal = terminal,
                jetToken = request.JetToken,
                cardHolderName = request.CardholderName,

            };
            var json = System.Text.Json.JsonSerializer.Serialize(payload);
            var resp = await http.PostAsync("v1/cards", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
            var body = await resp.Content.ReadAsStringAsync();

            try
            {
                using var doc = System.Text.Json.JsonDocument.Parse(body);
                var root = doc.RootElement;
                string? idUser = null;
                string? tokenUser = null;
                if (root.TryGetProperty("idUser", out var idUserEl)) idUser = idUserEl.GetString();
                if (root.TryGetProperty("tokenUser", out var tokenEl)) tokenUser = tokenEl.GetString();
                if (!string.IsNullOrEmpty(idUser) && !string.IsNullOrEmpty(tokenUser))
                {
                    return Ok(new { idUser, tokenUser, raw = root });
                }
            }
            catch { /* ignore parse errors and relay raw */ }

            return StatusCode((int)resp.StatusCode, string.IsNullOrWhiteSpace(body) ? new { error = "Respuesta vacía de PAYCOMET" } : body);
        }

        [HttpPost("charge-token")]
        public async Task<IActionResult> ChargeToken([FromBody] ChargeTokenRequest request)
        {
            if (!IsAuthorized()) return Unauthorized(new { error = "invalid api key" });
            if (string.IsNullOrWhiteSpace(request.idUser) || string.IsNullOrWhiteSpace(request.tokenUser))
                return BadRequest(new { error = "idUser y tokenUser requeridos" });
            return Ok(new { status = "APPROVED", amount = request.amount, currency = request.currency ?? "EUR" });
        }

        private bool IsAuthorized()
        {
            var required = _config["APP_API_KEY"];
            // Si no se configuró APP_API_KEY, permitir acceso sin auth
            if (string.IsNullOrWhiteSpace(required)) return true;
            if (!Request.Headers.TryGetValue("x-api-key", out var provided)) return false;
            return string.Equals(provided.ToString(), required, StringComparison.Ordinal);
        }
    }

    public class AddCardRequest
    {
        public string? JetToken { get; set; }
        public string? CardholderName { get; set; }
    }

    public class ChargeTokenRequest
    {
        public string idUser { get; set; } = string.Empty;
        public string tokenUser { get; set; } = string.Empty;
        public int amount { get; set; }
        public string? currency { get; set; }
        public string? orderId { get; set; }
        public bool? mit { get; set; } = true;
    }
}
