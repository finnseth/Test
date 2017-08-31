using Dualog.PortalService.Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dualog.PortalService.Core;
using Newtonsoft.Json;
using NJsonSchema.Converters;
using System.Runtime.Serialization;

namespace Dualog.PortalService.Controllers.Organization.Shipping.Ship.Model
{
    [JsonConverter(typeof(JsonInheritanceConverter), "ShipModel")]
    [KnownType(typeof(ShipModel))]
    public class ShipModel : DataAnnotationsValidatable
    {
        public long Id { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters long.")]
        [NoWhitespace]
        public string Name { get; set; }

        public string Company { get; set; }

        [Range(1, 10)]
        public short AccountEnabled { get; set; }

        [StringLength(10, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 10 characters long.")]
        public string InstalledVersion { get; set; }

        public bool? IsOfficeInstallation { get; set; }

        public string Phone { get; set; }

        public string DatabaseVersion { get; set; }

        public string HardwareID { get; set; }
        
        public string RadioCallSignal { get; set; }

        [StringLength(20, MinimumLength = 1, ErrorMessage = "IMO must be between 1 and 20 characters long.")]
        [NoWhitespace]
        public string ImoNumber { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 50 characters long.")]
        [NoWhitespace]
        public string DialinPassword { get; set; }
    }
}
