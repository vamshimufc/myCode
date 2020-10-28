using FluentValidation;
using wdhrtosis.Models;

namespace wdhrtosis.Validation
{
    public class WorkdayToPersonValidator : AbstractValidator<Email>
    {
        public WorkdayToPersonValidator()
        {
            RuleFor(t => t.EmployeeId).NotEmpty().WithMessage("EmployeeId is required");
            RuleFor(t => t.UniversalId).NotEmpty().WithMessage("UniversalId is required");
            RuleFor(t => t.EmailAddress).NotEmpty().WithMessage("Email Address is required");
            RuleFor(t => t.EmailType).NotEmpty().WithMessage("Email Type is required");
            RuleFor(t => t.PrimaryIndicator).NotEmpty().WithMessage("Primary Indicator is required");
        }
    }
}
