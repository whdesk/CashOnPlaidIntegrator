using Refit;

namespace PlaidApi.Pibisi
{
    public class PibisiCustomerBasicRequest
    {
        public string Country { get; set; }
        public string NationalId { get; set; }
        public string FullName { get; set; }
        public string BirthDate { get; set; } // "YYYY-MM-DD"
        public string City { get; set; }
        public string ZipCode { get; set; }
    }

    public class PibisiCustomerRequest
    {
        [AliasAs("person")]
        public string PersonType { get; set; } // "P" o "E"
        [AliasAs("name.full")]
        public string FullName { get; set; }
        [AliasAs("id.national")]
        public string NationalId { get; set; }
        public string NationalIdCountry { get; set; }
        [AliasAs("birth.date")]
        public string BirthDate { get; set; } // "YYYY-MM-DD"
        [AliasAs("email")]
        public string Email { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
    }
 

    public class PibisiAccountConfig
    {
        public object behaviour_analytics { get; set; }
        public object person_analytics { get; set; }
    }

    public class PibisiAccount
    {
        public string uuid { get; set; }
        public PibisiAccountConfig config { get; set; }
        // Otros campos según sea necesario
    }

    public class PibisiAccountDataItem
    {
        public string role { get; set; }
        public PibisiAccount account { get; set; }
    }

    public class PibisiAccountsResponse
    {
        public List<PibisiAccountDataItem> data { get; set; }
        public object meta { get; set; }
    }

    public class PibisiCustomerResponse
    {
        public PibisiCustomerData data { get; set; }
        public object meta { get; set; }
    }

    public class PibisiCustomerData
    {
        public string uuid { get; set; }
        public string status { get; set; }
        public string risk { get; set; }
        public PibisiScoring scoring { get; set; }
        public List<PibisiPoiInfo> info { get; set; }
    }

    public class PibisiScoring
    {
        public int value { get; set; }
        public PibisiFlags flags { get; set; }
    }

    public class PibisiFlags
    {
        public bool has_adverse_info { get; set; }
        public bool has_matches { get; set; }
        public bool has_media { get; set; }
        public string has_media_date { get; set; }
        public bool is_high_risk { get; set; }
        public bool is_pep { get; set; }
        public bool was_pep { get; set; }
        public string was_pep_date { get; set; }
        public bool is_sanctioned { get; set; }
        public bool was_sanctioned { get; set; }
        public string was_sanctioned_date { get; set; }
        public bool is_terrorist { get; set; }
    }

    public class PibisiPoiInfo
    {
        public string type { get; set; }
        public object content { get; set; }
    }
}
