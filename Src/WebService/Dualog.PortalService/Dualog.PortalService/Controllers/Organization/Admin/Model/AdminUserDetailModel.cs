using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Dualog.PortalService.Core.Validation;
using Dualog.Data.Oracle.Shore.Model;




namespace Dualog.PortalService.Controllers.Organization.Admin.Model
{
    public class AdminUserDetailModel : AdminUserModel
    {

        [Required(AllowEmptyStrings = true, ErrorMessage = "Email is required.")]
        [NoWhitespace]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(200, ErrorMessage = "Email cannot be longer than 200 characters.")]
        public string Email { get; set; }

        [StringLength(20, ErrorMessage = "PhoneNumber cannot be longer than 20 characters.")]
        public string Phone { get; set; }

        [StringLength(100, ErrorMessage = "Address cannot be longer than 200 characters.")]
        public string Address { get; set; }

        public DateTime? Created { get; set; }
    }
}
