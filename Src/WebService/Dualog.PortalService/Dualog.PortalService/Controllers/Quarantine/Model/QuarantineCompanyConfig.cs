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

namespace Dualog.PortalService.Controllers.Quarantine.Model
{
    [JsonConverter(typeof(JsonInheritanceConverter), "QuarantineCompanyConfig")]
    [KnownType(typeof(QuarantineVesselConfig))]
    public class QuarantineCompanyConfig : DataAnnotationsValidatable
    {
        public long QuarantineId { get; set; }
        public bool OnHoldStationaryUser { get; set; }
        public bool OnHoldCrew { get; set; }
        public bool NotificationOnHoldOriginal { get; set; }
        public bool NotificationOnHoldRecipient { get; set; }
        public bool NotificationOnHoldPostmaster { get; set; }


        [StringLength(2000, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 2000 characters long.")]
        [NoWhitespace]
        public string NotificationOnHoldAdmins { get; set; }

        [StringLength(2000, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 2000 characters long.")]
        [NoWhitespace]
        public string NotificationSender { get; set; }

        [Range(0, 99999)]
        public int MaxBodyLength { get; set; }

        [Range(0, 9999)]
        public int OnHoldDuration { get; set; }

        public static QuarantineCompanyConfig FromDsQuarantine(DsQuarantine quarantine)
        {
            var config = (QuarantineCompanyConfig) QuarantineVesselConfig.FromDsQuarantine(quarantine);

            return config;
        }
    }
}
