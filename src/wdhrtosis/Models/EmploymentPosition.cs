using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace wdhrtosis.Models
{
    public class EmploymentPosition
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public string UniversalId { get; set; }
        public int PrimaryJobIndicator { get; set; }
        public string EmployeeType { get; set; }
        public string JobCode { get; set; }
        public string JobTitle { get; set; }
        public string JobClassificationGroup { get; set; }
        public string BusinessTitle { get; set; }
        public string JobActiveIndicator { get; set; }
        public string JobStatus { get; set; }
        public string JobBenefitsActiveIndicator { get; set; }
        public string Manager { get; set; }
        public DateTimeOffset? JobEffectiveDate { get; set; }
        public DateTimeOffset? JobEndDate { get; set; }
        public string BuildingNumber { get; set; }
        public string BuildingName { get; set; }
        public string RoomNumber { get; set; }
        public string FloorNumber { get; set; }
        public string TimeType { get; set; }

        [Column(TypeName = "decimal(18, 0)")]
        public decimal? JobFTE { get; set; }
        public string PayGroup { get; set; }
        public string TerminationReason { get; set; }
        public DateTimeOffset? AnnualWorkPeriodStartDate { get; set; }
        public DateTimeOffset? AnnualWorkPeriodEndDate { get; set; }
        public int WorkPeriodPercentOfYear { get; set; }
        public DateTimeOffset? DisbursementPlanPeriodStartDate { get; set; }
        public DateTimeOffset? DisbursementPlanPeriodEndDate { get; set; }
        public string PayType { get; set; }
        public string ImportCorrelationId { get; set; }
        public DateTime ImportCreatedDate { get; set; }
        public DateTime ImportLastUpdatedDate { get; set; }
        public bool ImportIsActiveRecord { get; set; }

    }
}
