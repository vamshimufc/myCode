using System;

namespace wdhrtosis.Models
{
    public class Contact
    {

        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public string UniversalId { get; set; }
        public string CountryAccessCode { get; set; }
        public string AreaCityCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Extension { get; set; }
        public string DeviceType { get; set; }
        public string PhoneType { get; set; }
        public string PhoneNumberFormatted { get; set; }
        public bool? PrimaryIndicator { get; set; }
        public bool? PublicIndicator { get; set; }
        public DateTime ImportCreatedDate { get; set; }
        public DateTime ImportLastUpdatedDate { get; set; }
        public bool ImportIsActiveRecord { get; set; }
    }
}
