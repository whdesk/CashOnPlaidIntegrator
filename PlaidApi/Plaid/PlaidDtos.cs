namespace PlaidApi.Plaid
{
    public class PlaidLinkTokenRequest
    {
        public string client_id { get; set; }
        public string secret { get; set; }
        public string client_name { get; set; }
        public object user { get; set; }
        public string[] products { get; set; }
        public string[] country_codes { get; set; }
        public string language { get; set; }
    }

    public class PlaidLinkTokenResponse
    {
        public string link_token { get; set; }
    }

    public class PlaidExchangeTokenRequest
    {
        public string client_id { get; set; }
        public string secret { get; set; }
        public string public_token { get; set; }
    }

    public class PlaidExchangeTokenResponse
    {
        public string access_token { get; set; }
        public string item_id { get; set; }
        public string request_id { get; set; }
    }
}
