using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NJsonSchema.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dualog.PortalService.Core;
using System.ComponentModel.DataAnnotations;
using Dualog.PortalService.Core.Validation;

namespace Dualog.PortalService.Controllers.Permissions.Model
{
    public enum PermissionOrigin
    {
        UserGroup = 1,
        User = 2
    }

    public class PermissionDetails
    {
        [JsonSchemaExtensionData( "key", true)]
        [Required( AllowEmptyStrings = true, ErrorMessage = "Name is required." )]
        [NoWhitespace]
        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public AccessRights AllowType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public PermissionOrigin Origin { get; set; }
    }
}
