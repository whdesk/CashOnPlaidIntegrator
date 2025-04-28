using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace PlaidApi.Controllers
{
    [ApiController]
    [Route("api/stripe")]
    public class StripePaymentController : ControllerBase
    {
        public StripePaymentController()
        {
            var stripeApiKey = Environment.GetEnvironmentVariable("STRIPE_SECRET_KEY");
            if (string.IsNullOrWhiteSpace(stripeApiKey))
            {
                throw new InvalidOperationException("La clave secreta de Stripe no está definida en la variable de entorno STRIPE_SECRET_KEY.");
            }
            StripeConfiguration.ApiKey = stripeApiKey;
        }

        [HttpPost("create-customer")]
        public ActionResult CreateCustomer([FromBody] CreateCustomerRequest request)
        {
            var customerOptions = new CustomerCreateOptions
            {
                Email = request.Email,
                Name = request.Name
            };
            var customerService = new CustomerService();
            var customer = customerService.Create(customerOptions);
            return Ok(new { customerId = customer.Id });
        }

        [HttpPost("attach-payment-method")]
        public ActionResult AttachPaymentMethod([FromBody] AttachPaymentMethodRequest request)
        {
            var paymentMethodService = new PaymentMethodService();
            paymentMethodService.Attach(request.PaymentMethodId, new PaymentMethodAttachOptions
            {
                Customer = request.CustomerId
            });

            var customerService = new CustomerService();
            customerService.Update(request.CustomerId, new CustomerUpdateOptions
            {
                InvoiceSettings = new CustomerInvoiceSettingsOptions
                {
                    DefaultPaymentMethod = request.PaymentMethodId
                }
            });

            return Ok();
        }

        [HttpPost("charge")]
        public ActionResult Charge([FromBody] ChargeRequest request)
        {
            var paymentIntentService = new PaymentIntentService();
            var paymentIntent = paymentIntentService.Create(new PaymentIntentCreateOptions
            {
                Amount = request.Amount,
                Currency = request.Currency,
                Customer = request.CustomerId,
                PaymentMethod = request.PaymentMethodId,
                OffSession = true,
                Confirm = true
            });
            return Ok(new { paymentIntentId = paymentIntent.Id });
        }
    }

    public class CreateCustomerRequest
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }

    public class AttachPaymentMethodRequest
    {
        public string CustomerId { get; set; }
        public string PaymentMethodId { get; set; }
    }

    public class ChargeRequest
    {
        public string CustomerId { get; set; }
        public string PaymentMethodId { get; set; }
        public long Amount { get; set; }
        public string Currency { get; set; }
    }
}
