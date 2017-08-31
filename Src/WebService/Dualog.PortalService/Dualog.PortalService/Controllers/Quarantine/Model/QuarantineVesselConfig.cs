using Dualog.Data.Oracle.Shore.Model;
using Dualog.PortalService.Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace Dualog.PortalService.Controllers.Quarantine.Model
{
    public class QuarantineVesselConfig : QuarantineCompanyConfig
    {
        public long VesselId { get; set; }

        public string VesselName { get; internal set; }
        public bool UseThisLevel { get; set; }


        public static new QuarantineVesselConfig FromDsQuarantine(DsQuarantine quarantine)
        {
            return new QuarantineVesselConfig
            {
                QuarantineId = quarantine.Id,
                MaxBodyLength = quarantine.MaxBodyLength,
                NotificationOnHoldAdmins = quarantine.NotificationOnHoldAdmins,
                NotificationOnHoldOriginal = quarantine.NotificationOnHoldOriginal,
                NotificationOnHoldPostmaster = quarantine.NotificationOnHoldPostmaster,
                NotificationOnHoldRecipient = quarantine.NotificationOnHoldRecipient,
                NotificationSender = quarantine.NotificationSender,
                OnHoldCrew = quarantine.OnHoldCrew,
                OnHoldDuration = quarantine.OnHoldDuration,
                OnHoldStationaryUser = quarantine.OnHoldStationaryUser,
                VesselId = quarantine.Vessel?.Id ?? 0,
                VesselName = quarantine.Vessel?.VesselName,
                UseThisLevel = quarantine.UseThisLevel
        };
        }

    }
}