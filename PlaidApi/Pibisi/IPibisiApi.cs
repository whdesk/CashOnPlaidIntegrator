using System.Threading.Tasks;
using Refit;

namespace PlaidApi.Pibisi
{
    public interface IPibisiApi
    {
        [Get("/users/me")]
        Task<string> GetCredentials([Header("X-AUTH-TOKEN")] string authToken);

        [Get("/users/me/accounts")]
        Task<string> GetAccounts([Header("X-AUTH-TOKEN")] string authToken);

        //[Multipart]
        //[Post("/accounts/{account}/customers/")]
        //Task<PibisiCustomerResponse> RegisterCustomer(
        //    [Header("X-AUTH-TOKEN")] string authToken,
        //    [AliasAs("person")] string personType,
        //    [AliasAs("name.full")] string fullName,
        //    [AliasAs("id.national")] string nationalId,
        //    [AliasAs("birth.date")] string birthDate
        //    // Agrega otros campos si lo necesitas
        //);
    }
}
