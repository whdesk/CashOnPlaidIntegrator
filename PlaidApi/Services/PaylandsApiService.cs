using System.Threading.Tasks;
using PlaidApi.Paylands;
using Microsoft.Extensions.Configuration;

namespace PlaidApi.Services
{
    public interface IPaylandsApiService
    {
        Task<string?> CreateCustomerAsync(string customerExtId);
    }

    public class PaylandsApiService : IPaylandsApiService
    {
        private readonly IPaylandsApi _paylandsApi;
        private readonly string _apiKey;
        private readonly string _serviceSignature;

        public PaylandsApiService(IPaylandsApi paylandsApi, IConfiguration config)
        {
            _paylandsApi = paylandsApi;
            _apiKey = config["PAYLANDS_API_KEY"] ?? "pk_test_3c140607778e1217f56ccb8b50540e00";
            _serviceSignature = config["PAYLANDS_SIGNATURE"] ?? "jE1NmMjThILbyr0Vhhso7v2s";
        }

        public async Task<string?> CreateCustomerAsync(string customerExtId)
{
    var body = new CreateCustomerRequestBody
    {
        signature = _serviceSignature,
        customer_ext_id = customerExtId
    };

            try
            {

                var response = await _paylandsApi.CreateCustomer(body);
                return response.Customer.token;
            }
            catch (Exception ex)
            {

                throw;
            }

}
    }
}
