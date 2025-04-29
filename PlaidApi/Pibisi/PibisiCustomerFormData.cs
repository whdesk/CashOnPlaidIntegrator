namespace PlaidApi.Pibisi
{
    public class PibisiCustomerFormData
    {
        [Refit.AliasAs("pois")]
        public string Pois { get; set; }
        // Si necesitas lifetime, descomenta la siguiente línea:
        // [Refit.AliasAs("lifetime")]
        // public int? Lifetime { get; set; }
    }
}
