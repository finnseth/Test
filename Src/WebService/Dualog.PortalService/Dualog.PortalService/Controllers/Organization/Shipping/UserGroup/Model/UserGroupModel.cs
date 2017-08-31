using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Dualog.PortalService.Core.Validation;
using Dualog.Data.Oracle.Shore.Model;



namespace Dualog.PortalService.Controllers.Organization.Shipping.UserGroup.Model
{
    public class UserGroupModel : DataAnnotationsValidatable
    {
        public long Id { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        public short? Rowstatus { get; set; }

        public long? VesselId { get; set; }

        public string VesselName { get; set; }

        public long CompanyId { get; set; }

        public string CompanyName { get; set; }

    }
}
