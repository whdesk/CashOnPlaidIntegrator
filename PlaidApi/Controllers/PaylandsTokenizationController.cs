using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using PlaidApi.Services;

namespace PlaidApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaylandsTokenizationController : ControllerBase
    {
        private readonly IPaylandsApiService _paylandsApiService;
        public PaylandsTokenizationController(IPaylandsApiService paylandsApiService)
        {
            _paylandsApiService = paylandsApiService;
        }

        [HttpPost("create-customer")]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request)
        {
            var token = await _paylandsApiService.CreateCustomerAsync(request.customer_ext_id);
            if (token == null)
                return StatusCode(502, new { error = "No se pudo obtener el token de Paylands" });
            return Ok(new { token });
        }

        [HttpPost("saved-card-notify")]
        public IActionResult SavedCardNotify([FromBody] SavedCardNotification notification)
        {
            // Aquí puedes guardar la notificación en base de datos, enviar un correo, etc.
            // Por ahora, solo la registramos en el log.
            Console.WriteLine($"[Paylands] SavedCard notification recibida: {System.Text.Json.JsonSerializer.Serialize(notification)}");
            return Ok(new { message = "Notificación recibida correctamente" });
        }

        public class SavedCardNotification
        {
            public string? cardToken { get; set; }
            public string? customerToken { get; set; }
            public string? brand { get; set; }
            public string? last4 { get; set; }
            public string? expiry { get; set; }
            public string? cardholder { get; set; }
            // Agrega otros campos según lo que envía Paylands
        }
    }

    public class CreateCustomerRequest
    {
        public string customer_ext_id { get; set; } = string.Empty;
    }
}
