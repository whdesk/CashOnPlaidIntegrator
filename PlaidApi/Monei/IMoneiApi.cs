using Refit;
using System.Net.Http;
using System.Threading.Tasks;

namespace PlaidApi.Monei
{
    public interface IMoneiApi
    {
        [Post("/payments")]
        Task<HttpResponseMessage> CreatePayment([Body] object payload);

        [Post("/payments/{id}/confirm")]
        Task<HttpResponseMessage> ConfirmPayment(string id, [Body] object payload);
    }
}
