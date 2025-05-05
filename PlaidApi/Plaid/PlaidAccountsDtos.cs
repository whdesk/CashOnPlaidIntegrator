using System.Text.Json.Serialization;

namespace PlaidApi.Plaid
{
    public class PlaidAccountsRequest
    {
        public string client_id { get; set; }
        public string secret { get; set; }
        public string access_token { get; set; }
    }

    public class PlaidAccountsResponse
    {
        public Account[] accounts { get; set; }
        public Item item { get; set; }
        public string request_id { get; set; }
    }

    public class PlaidAuthRequest
    {
        public string client_id { get; set; }
        public string secret { get; set; }
        public string access_token { get; set; }
    }

    public class PlaidAuthResponse
    {
        public Account[] accounts { get; set; }
        public Numbers numbers { get; set; }
        public Item item { get; set; }
        public string request_id { get; set; }
    }

    public class PlaidTransactionsRequest
    {
        public string client_id { get; set; }
        public string secret { get; set; }
        public string access_token { get; set; }
        public string start_date { get; set; } // YYYY-MM-DD
        public string end_date { get; set; }   // YYYY-MM-DD

        public Options options { get; set; }
    }

    public class Options
    {
        public int count { get; set; } = 100;
        public int offset { get; set; } = 0;
    }

    public class PlaidTransactionsResponse
    {
        public Transaction[] transactions { get; set; }
        public Account[] accounts { get; set; }
        public Item item { get; set; }
        public int total_transactions { get; set; }
        public string request_id { get; set; }
    }

    public class Transaction
    {
        public string transaction_id { get; set; }
        public string account_id { get; set; }
        public string name { get; set; }
        public decimal amount { get; set; }
        public DateTime date { get; set; }
        public string merchant_name { get; set; }
        public bool pending { get; set; }
        public string iso_currency_code { get; set; }
        public string payment_channel { get; set; }
        public PersonalFinanceCategory personal_finance_category { get; set; }
    }

    public class PersonalFinanceCategory
    {
        public string primary { get; set; }
        public string detailed { get; set; }
        public string confidence_level { get; set; }
    }

    public class Account
    {
        public string account_id { get; set; }
        public string name { get; set; }
        public string official_name { get; set; }
        public string subtype { get; set; }
        public string type { get; set; }
        public Balances balances { get; set; }
        public Numbers numbers { get; set; } // Solo si usas /auth/get
    }

    public class Balances
    {
        public decimal? available { get; set; }
        public decimal? current { get; set; }
        public string iso_currency_code { get; set; }
        public string unofficial_currency_code { get; set; }
    }

    public class Numbers
    {
        public string account { get; set; }
        public string routing { get; set; }
        public string wire_routing { get; set; }
    }

    public class AccessTokenRequestDto
    {
        [JsonPropertyName("public_token")]
        public string AccessToken { get; set; } = string.Empty;
    }

    public class Item
    {
        public string item_id { get; set; }
        public string institution_id { get; set; }
    }
}
