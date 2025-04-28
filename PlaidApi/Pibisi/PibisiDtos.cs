using Refit;

namespace PlaidApi.Pibisi
{
    public class PibisiCustomerRequest
    {
        [AliasAs("person")]
        public string PersonType { get; set; } // "P" o "E"
        [AliasAs("name.full")]
        public string FullName { get; set; }
        [AliasAs("id.national")]
        public string NationalId { get; set; }
        [AliasAs("birth.date")]
        public string BirthDate { get; set; } // "YYYY-MM-DD"
        // Puedes agregar más campos según tu necesidad
    }

    public class PibisiCustomerResponse
    {
        public object data { get; set; }
        public object meta { get; set; }
    }
}
