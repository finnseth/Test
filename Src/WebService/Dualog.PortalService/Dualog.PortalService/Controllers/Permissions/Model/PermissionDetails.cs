using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dualog.PortalService.Core;
using Dualog.PortalService.Core.Validation;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NJsonSchema.Annotations;

namespace Dualog.PortalService.Controllers.Permissions.Model
{
    public class PermissionDetails
    {
        [JsonSchemaExtensionData( "key", true)]
        [Required( AllowEmptyStrings = true, ErrorMessage = "Name is required." )]
        [NoWhitespace]
        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public AccessRights AllowType { get; set; }

        public string Origin { get; set; }
    }
}
