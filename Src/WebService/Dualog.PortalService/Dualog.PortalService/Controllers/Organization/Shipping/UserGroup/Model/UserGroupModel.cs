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
        [StringLength(100, ErrorMessage = ValidationStrings.StringToLong)]
        public string Name { get; set; }

        [Range(0, 99)]
        public short? Rowstatus { get; set; }

        public long? VesselId { get; set; }

        public string VesselName { get; set; }

        public long CompanyId { get; set; }

        public string CompanyName { get; set; }

    }
}
