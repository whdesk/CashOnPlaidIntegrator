using Refit;
using System.Threading.Tasks;

namespace PlaidApi.Paylands
{
    public interface IPaylandsApi
    {
        [Post("/customer")]
        Task<CreateCustomerResponse> CreateCustomer([Body] CreateCustomerRequestBody body);
    }

    public class CreateCustomerRequestBody
    {
        public string signature { get; set; } = string.Empty;
        public string customer_ext_id { get; set; } = string.Empty;
    }

    public class CreateCustomerResponse
    {
        public string message { get; set; } = string.Empty;
        public int code { get; set; }
        public string current_time { get; set; } = string.Empty;
        public CustomerData Customer { get; set; } = new CustomerData();
    }

    public class CustomerData
    {
        public string external_id { get; set; } = string.Empty;
        public string token { get; set; } = string.Empty;
    }
}
