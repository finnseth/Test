using Dualog.PortalService.Core.Validation;
using Newtonsoft.Json;
using NJsonSchema.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Dualog.Data.Oracle.Shore.Model;

namespace Dualog.PortalService.Controllers.Email.Setup.Domain.Model
{
    [JsonConverter(typeof(JsonInheritanceConverter), "DomainModel")]
    public class DomainModel : DataAnnotationsValidatable
    {
        public long Id { get; set; }

        [StringLength(30, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 30 characters long.")]
        [NoWhitespace]
        public string DomainPrefix { get; set; }

        public IEnumerable<CustomDomainModel> CustomDomains { get; set; }

    }
}
