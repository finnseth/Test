using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Dualog.PortalService.Core.Validation
{
    public abstract class DataAnnotationsValidatable : IValidatable
    {
        public bool Validate(out string message)
        {
            var validationResults = new List<ValidationResult>();
            if (Validator.TryValidateObject(this, new ValidationContext(this), validationResults, true) == false)
            {
                message = validationResults.First().ErrorMessage;
                return false;

            }

            message = null;
            return true;
        }
    }
}
