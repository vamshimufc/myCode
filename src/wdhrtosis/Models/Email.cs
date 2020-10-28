using System;

namespace wdhrtosis.Models
{
    public class Email
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public string UniversalId { get; set; }
        public string EmailAddress { get; set; }
        public string EmailComment { get; set; }
        public string EmailType { get; set; }
        public bool? PrimaryIndicator { get; set; }
        public bool? PublicIndicator { get; set; }
        public DateTime ImportCreatedDate { get; set; }
        public DateTime ImportLastUpdatedDate { get; set; }
        public bool ImportIsActiveRecord { get; set; }

    }
}
