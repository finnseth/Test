using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dualog.PortalService.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MultipleEmailAddresses : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            string text = value as string;
            if(text == null)
                return ValidationResult.Success;

            // Emails must be separated with ','
            string[] emails = text.Split(',');
            foreach (string email in emails)
            {
                var trimmedemail = email.Trim();
                if (!new EmailAddressAttribute().IsValid(trimmedemail)) return new ValidationResult("Invalid email address: " + trimmedemail);
            }

            return ValidationResult.Success;
        }
    }
}
