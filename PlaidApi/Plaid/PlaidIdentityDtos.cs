namespace PlaidApi.Plaid
{
    public class PlaidIdentityRequest
    {
        public string client_id { get; set; }
        public string secret { get; set; }
        public string access_token { get; set; }
    }

    public class PlaidIdentityResponse
    {
        public IdentityAccount[] accounts { get; set; }
        public IdentityItem item { get; set; }
        public string request_id { get; set; }
    }

    public class IdentityAccount
    {
        public string account_id { get; set; }
        public Identity[] owners { get; set; }
    }

    public class Identity
    {
        public IdentityNames[] names { get; set; }
        public IdentityIdentification[] identifications { get; set; }
    }

    public class IdentityNames
    {
        public string full_name { get; set; }
    }

    public class IdentityIdentification
    {
        public string type { get; set; } // Ej: "passport", "dni", "ssn", etc.
        public string value { get; set; }
        public string status { get; set; }
    }

    public class IdentityItem
    {
        public string item_id { get; set; }
    }
}
