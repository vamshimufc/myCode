using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace wdhrtosis.Models
{
    public class EmploymentProfile
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public string UniversalId { get; set; }
        public string Status { get; set; }
        public string Campus { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        public decimal? TotalFTE { get; set; }
        public DateTimeOffset? ContinuousServiceDate { get; set; }
        public string OptOutWURecord { get; set; }
        public string LocationHierarchy { get; set; }
        public string OptOutTotalDirectoryInfo { get; set; }
        public DateTimeOffset? OrigHireDate { get; set; }
        public DateTimeOffset? TerminationDate { get; set; }
        public string WorkerType { get; set; }
        public string CampusMailStop { get; set; }
        public DateTimeOffset? HireDate { get; set; }
        public int? RetireeIndicator { get; set; }
        public string ImportCorrelationId { get; set; }
        public DateTime ImportCreatedDate { get; set; }
        public DateTime ImportLastUpdatedDate { get; set; }
        public bool ImportIsActiveRecord { get; set; }
        public string WorkerPrimaryPositionType { get; set; }

    }
}
