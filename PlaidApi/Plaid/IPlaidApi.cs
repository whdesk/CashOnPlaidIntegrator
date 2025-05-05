using Refit;
using System.Threading.Tasks;

namespace PlaidApi.Plaid
{
    public interface IPlaidApi
    {
        [Post("/link/token/create")]
        Task<PlaidLinkTokenResponse> CreateLinkToken([Body] PlaidLinkTokenRequest request);

        [Post("/item/public_token/exchange")]
        Task<PlaidExchangeTokenResponse> ExchangePublicToken([Body] PlaidExchangeTokenRequest request);

        [Post("/auth/get")]
        Task<string> GetAuth([Body] PlaidAuthRequest request);

        [Post("/identity/get")]
        Task<string> GetIdentity([Body] PlaidIdentityRequest request);

        [Post("/transactions/get")]
        Task<PlaidTransactionsResponse> GetTransactions([Body] PlaidTransactionsRequest request);
    }
}
