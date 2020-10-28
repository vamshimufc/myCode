using Microsoft.Extensions.Logging;
using System.Linq;
using wdhrtosis.Models;

namespace wdhrtosis.Validation
{
    public interface IValidationManager
    {
        bool IsValid(Email email);
    }

    public class ValidationManager : IValidationManager
    {
        private readonly ILogger<ValidationManager> _logger;

        public ValidationManager(ILogger<ValidationManager> logger)
        {
            _logger = logger;
        }

        public bool IsValid(Email email)
        {
            var validator = new WorkdayToPersonValidator();
            var result = validator.Validate(email);

            if (!result.IsValid)
            {
                _logger.LogWarning($"Validation failed for Email with EmployeeId SupplierID: {email.EmployeeId}");
                return false;
            }

            return true;
        }
    }
}
