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

    public class PibisiSubjectFindMatch
    {
        public string Uuid { get; set; }
        public string FullName { get; set; }
        public string NationalId { get; set; }
        public string NationalIdCountry { get; set; }
        public string BirthDate { get; set; }
        public double Similarity { get; set; }
        public string Status { get; set; } // Ej: "blacklist", "match", etc
    }

    // NUEVO: DTO fiel al JSON real de /subjects/find
    public class PibisiSubjectFindRootResponse
    {
        public PibisiSubjectFindData data { get; set; }
    }

    public class PibisiSubjectFindData
    {
        public PibisiSearchResult search_result { get; set; }
        public List<PibisiSubjectMatch> matches { get; set; }
    }

    public class PibisiSearchResult
    {
        public string uuid { get; set; }
        public double threshold { get; set; }
        public string status { get; set; }
        public List<PibisiPoi> pois { get; set; }
        public List<PibisiSearchMatch> matches { get; set; }
    }

    public class PibisiPoi
    {
        public string type { get; set; }
        public object content { get; set; }
    }

    public class PibisiSearchMatch
    {
        public string subject { get; set; }
        public double similarity { get; set; }
        public double cardinality { get; set; }
        public object similarity_vector { get; set; }
    }

    public class PibisiSubjectMatch
    {
        public string uuid { get; set; }
        public List<PibisiSubjectInfo> info { get; set; }
        public double similarity { get; set; }
        public double cardinality { get; set; }
        public object similarity_vector { get; set; }
        public PibisiScoring scoring { get; set; } // Ahora tipado
                                                   
    }

    public class PibisiSubjectInfo
    {
        public string group { get; set; }
        public string uuid { get; set; }
        public string type { get; set; }
        public object content { get; set; }
    }

    // DTO anterior (puede quedar para compatibilidad)
    public class PibisiSubjectFindResponse
    {
        public List<PibisiSubjectFindMatch> Data { get; set; }
        public object Meta { get; set; }
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
        public bool is_pep { get; set; }
        public bool was_pep { get; set; }
        public bool is_sanctioned { get; set; }
        public bool was_sanctioned { get; set; }
        public bool is_terrorist { get; set; }
        public bool is_high_risk { get; set; }
    }

    public class PibisiFlags
    {
        public bool is_pep { get; set; }
        public bool was_pep { get; set; }
        public bool is_sanctioned { get; set; }
        public bool was_sanctioned { get; set; }
        public bool is_terrorist { get; set; }
        public bool is_high_risk { get; set; }
    }

    public class PibisiPoiInfo
    {
        public string type { get; set; }
        public object content { get; set; }
    }
}
