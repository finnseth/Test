using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dualog.PortalService.Core;
using Dualog.PortalService.Core.Validation;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NJsonSchema.Annotations;

namespace Dualog.PortalService.Controllers.Email.Setup.Domain.Model
{
    public class CustomDomainModel
    {
        public long Id { get; set; }

        public long CompanyId { get; set; }

        [StringLength(200, MinimumLength = 1, ErrorMessage = "Domain must be between 1 and 200 characters long.")]
        [NoWhitespace]
        public string Domain { get; set; }
    }
}
