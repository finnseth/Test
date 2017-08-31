using Dualog.PortalService.Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dualog.PortalService.Controllers.Companies.Model
{
    public class CompanyInformation : DataAnnotationsValidatable
    {
        public long Id { get; set; }

        [Required( AllowEmptyStrings = true, ErrorMessage = "Name is required." )]
        [StringLength( 100, ErrorMessage = "Name cannot ve longer than 100 characters." )]
        [NoWhitespace]
        public string Name { get; set; }

        [Required( AllowEmptyStrings = true, ErrorMessage = "Manager is required." )]
        [StringLength( 100, ErrorMessage = "Manager cannot ve longer than 100 characters." )]
        [NoWhitespace]
        public string Manager { get; set; }

        [Required( AllowEmptyStrings = true, ErrorMessage = "Email is required." )]
        [NoWhitespace]
        [EmailAddress( ErrorMessage = "Invalid email address." )]
        [StringLength( 200, ErrorMessage = "Email cannot be longer than 200 characters." )]
        public string Email { get; set; }

        [NoWhitespace]
        [StringLength( 100, ErrorMessage = "Address cannot be longer than 100 characters." )]
        public string Address { get; set; }

        [NoWhitespace]
        [StringLength( 20, ErrorMessage = "PhoneNumber cannot be longer than 20 characters." )]
        public string PhoneNumber { get; set; }
    }
}
