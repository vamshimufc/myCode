
using System;

namespace wdhrtosis.Models
{
    public class Organization
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public string UniversalId { get; set; }
        public string OrganizationReferenceID { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationCode { get; set; }
        public string OrganizationType { get; set; }
        public string ImportCorrelationId { get; set; }
        public DateTime ImportCreatedDate { get; set; }
        public DateTime ImportLastUpdatedDate { get; set; }
        public bool ImportIsActiveRecord { get; set; }

    }
}
