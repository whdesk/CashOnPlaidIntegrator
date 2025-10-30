using Microsoft.AspNetCore.Mvc;
using PlaidApi.Monei;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PlaidApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoneiController : ControllerBase
    {
        private readonly IMoneiApi _moneiApi;
        private readonly IConfiguration _config;

        public MoneiController(IMoneiApi moneiApi, IConfiguration config)
        {
            _moneiApi = moneiApi;
            _config = config;
        }

        [HttpPost("create-payment")]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest request)
        {
            if (!IsAuthorized()) return Unauthorized(new { error = "invalid api key" });
            request ??= new CreatePaymentRequest();
            if (request.amount <= 0) request.amount = 110;
            if (string.IsNullOrWhiteSpace(request.currency)) request.currency = "EUR";
            if (string.IsNullOrWhiteSpace(request.orderId)) request.orderId = $"ORDER-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";
            if (string.IsNullOrWhiteSpace(request.callbackUrl)) request.callbackUrl = _config["MONEI_CALLBACK_URL"] ?? "https://example.com/monei/callback";

            var payload = new
            {
                amount = request.amount,
                currency = request.currency,
                orderId = request.orderId,
                description = request.description,
                customer = string.IsNullOrWhiteSpace(request.customerEmail) && string.IsNullOrWhiteSpace(request.customerName)
                    ? null
                    : new { email = request.customerEmail, name = request.customerName },
                callbackUrl = request.callbackUrl,
                paymentToken = string.IsNullOrWhiteSpace(request.paymentToken) ? null : request.paymentToken,
                sessionId = string.IsNullOrWhiteSpace(request.sessionId) ? null : request.sessionId
            };

            var resp = await _moneiApi.CreatePayment(payload);
            var body = await resp.Content.ReadAsStringAsync();
            return StatusCode((int)resp.StatusCode, body);
        }

        [HttpPost("confirm-payment")]
        public async Task<IActionResult> ConfirmPayment([FromBody] ConfirmPaymentRequest request)
        {
            if (!IsAuthorized()) return Unauthorized(new { error = "invalid api key" });
            if (string.IsNullOrWhiteSpace(request.paymentId))
            {
                return BadRequest(new { error = "paymentId es requerido" });
            }
            if (string.IsNullOrWhiteSpace(request.paymentToken))
            {
                return BadRequest(new { error = "paymentToken es requerido" });
            }

            var payload = new
            {
                paymentToken = request.paymentToken,
                sessionId = string.IsNullOrWhiteSpace(request.sessionId) ? null : request.sessionId,
                generatePaymentToken = request.generatePaymentToken ?? true
            };

            var resp = await _moneiApi.ConfirmPayment(request.paymentId, payload);
            var body = await resp.Content.ReadAsStringAsync();
            return StatusCode((int)resp.StatusCode, body);
        }

        private bool IsAuthorized()
        {
            var required = _config["APP_API_KEY"];
            if (string.IsNullOrWhiteSpace(required)) return false;
            if (!Request.Headers.TryGetValue("x-api-key", out var provided)) return false;
            return string.Equals(provided.ToString(), required, StringComparison.Ordinal);
        }
    }

    public class CreatePaymentRequest
    {
        public int amount { get; set; }
        public string currency { get; set; } = "EUR";
        public string orderId { get; set; } = string.Empty;
        public string? description { get; set; }
        public string? customerEmail { get; set; }
        public string? customerName { get; set; }
        public string callbackUrl { get; set; } = string.Empty;
        public string? paymentToken { get; set; }
        public string? sessionId { get; set; }
    }

    public class ConfirmPaymentRequest
    {
        public string paymentId { get; set; } = string.Empty;
        public string paymentToken { get; set; } = string.Empty;
        public string? sessionId { get; set; }
        public bool? generatePaymentToken { get; set; } = true;
    }
}
