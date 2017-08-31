using System;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Dualog.PortalService.Core.Validation;
using Dualog.Data.Oracle.Shore.Model;



namespace Dualog.PortalService.Controllers.Organization.Shipping.User.Model
{
    public class UserModel : DataAnnotationsValidatable
    {
        public long Id { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "Email is required.")]
        [NoWhitespace]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(200, ErrorMessage = "Email cannot be longer than 200 characters.")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        public int Rowstatus { get; set; }

        public bool IsVesselUser { get; set; }

        public long? VesselId { get; set; }

        public string VesselName { get; set; }

        public long CompanyId { get; set; }

        public string CompanyName { get; set; }
    }
}