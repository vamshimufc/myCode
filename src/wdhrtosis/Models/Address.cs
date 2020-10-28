using System;


namespace wdhrtosis.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public string UniversalId { get; set; }
        public string AddressType { get; set; }
        public string AddressId { get; set; }
        public DateTimeOffset? AddressEffectiveDate { get; set; }
        public bool? DefaultedBusinessAddress { get; set; }
        public bool? PrimaryIndicator { get; set; }
        public bool? PublicIndicator { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string AddressLine5 { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string StateProvinceCode { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public DateTime ImportCreatedDate { get; set; }
        public DateTime ImportLastUpdatedDate { get; set; }
        public bool ImportIsActiveRecord { get; set; }

    }
}
