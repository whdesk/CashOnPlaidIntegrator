using System.Collections.Generic;

namespace PlaidApi.Plaid
{
    public class SandboxUserParams
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public decimal MonthlyIncome { get; set; }
        public decimal MonthlyRent { get; set; }
        public decimal MonthlyUtilities { get; set; }
        public bool HasPension { get; set; }
        public decimal? PensionAmount { get; set; }
        public bool HasRentalIncome { get; set; }
        public decimal? RentalIncome { get; set; }
    }
}
