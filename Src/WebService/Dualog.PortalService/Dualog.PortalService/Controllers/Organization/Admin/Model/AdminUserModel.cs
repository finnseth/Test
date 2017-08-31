using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Dualog.PortalService.Core.Validation;
using Dualog.Data.Oracle.Shore.Model;




namespace Dualog.PortalService.Controllers.Organization.Admin.Model
{
    public class AdminUserModel : DataAnnotationsValidatable
    {

        public long Id { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }

    }
}
