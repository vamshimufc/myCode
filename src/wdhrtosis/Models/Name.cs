using System;

namespace wdhrtosis.Models
{
    public class Name
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public string UniversalId { get; set; }
        public string NationalId { get; set; }
        public string Prefix { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string PreferredPrefix { get; set; }
        public string PreferredFirstName { get; set; }
        public string PreferredMiddleName { get; set; }
        public string PreferredLastName { get; set; }
        public string PreferredSuffix { get; set; }
        public string ReportingName { get; set; }
        public string Pronunciation { get; set; }
        public string Gender { get; set; }
        public DateTimeOffset? DateofBirth { get; set; }
        public DateTime ImportCreatedDate { get; set; }
        public DateTime ImportLastUpdatedDate { get; set; }
        public bool ImportIsActiveRecord { get; set;}


    }
}
