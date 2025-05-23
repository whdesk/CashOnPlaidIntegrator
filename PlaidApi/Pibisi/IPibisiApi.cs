using System.Threading.Tasks;
using Refit;

namespace PlaidApi.Pibisi
{
    public interface IPibisiApi
    {
        [Get("/users/me")]
        Task<string> GetCredentials([Header("X-AUTH-TOKEN")] string authToken);

        [Get("/users/me/accounts")]
        Task<PibisiAccountsResponse> GetAccounts([Header("X-AUTH-TOKEN")] string authToken);

        [Headers("Content-Type: application/x-www-form-urlencoded")]
        [Post("/accounts/{account}/customers")]
        Task<PibisiCustomerResponse> RegisterCustomer(
            [AliasAs("account")] string accountId,
            [Body(BodySerializationMethod.UrlEncoded)] PibisiCustomerFormData formData
        );

        [Get("/accounts/{account}/customers/{customer}")]
        Task<PibisiCustomerResponse> GetCustomer(
            [AliasAs("account")] string accountId,
            [AliasAs("customer")] string customer
        );

        [Headers("Content-Type: application/x-www-form-urlencoded")]
        [Post("/accounts/{account}/subjects/find")]
        Task<PibisiSubjectFindRootResponse> FindSubjectAsync(
            [AliasAs("account")] string accountId,
            [Body(BodySerializationMethod.UrlEncoded)] PibisiCustomerFormData formData,
            [Header("X-AUTH-TOKEN")] string authToken
        );
    }
}
