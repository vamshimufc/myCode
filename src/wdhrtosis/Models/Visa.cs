using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wdhrtosis.Models
{
    public class Visa
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public string UniversalId { get; set; }
        public string VisaNumber { get; set; }
        public string VisaType { get; set; }
        public string VisaCountry { get; set; }
        public string VisaPermitStatus { get; set; }
        public string VisaPermitDuration { get; set; }
        public string VisaPermitDurationType { get; set; }
        public string VisaIssuingAuthority { get; set; }
        public DateTimeOffset? VisaIssueDate { get; set; }
        public DateTimeOffset? VisaExpiryDate { get; set; }
        public DateTimeOffset? VisaVerificationDate { get; set; }
        public DateTime ImportCreatedDate { get; set; }
        public DateTime ImportLastUpdatedDate { get; set; }
        public bool ImportIsActiveRecord { get; set; }
    }
}
