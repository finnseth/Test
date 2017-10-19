using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dualog.PortalService.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class NoWhitespaceAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string text = value as string;
            if (text == null)
                return ValidationResult.Success;

            if (text.Length > 0 && string.IsNullOrWhiteSpace(text) == true)
                return new ValidationResult($"The value of {validationContext.MemberName} is invalid.");

            return ValidationResult.Success;
        }
    }
}
