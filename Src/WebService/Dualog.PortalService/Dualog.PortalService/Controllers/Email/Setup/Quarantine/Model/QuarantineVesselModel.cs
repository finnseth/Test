using Dualog.Data.Oracle.Shore.Model;
using Dualog.PortalService.Core.Validation;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Dualog.PortalService.Controllers.Email.Setup.Quarantine.Model
{
    [KnownType(typeof(QuarantineCompanyModel))]

    public class QuarantineVesselModel : QuarantineCompanyModel
    {
        public long VesselId { get; set; }

        public string VesselName { get; internal set; }
        public bool UseThisLevel { get; set; }


        public static new QuarantineVesselModel FromDsQuarantine(DsQuarantine quarantine)
        {

            //           var vesselconfig = (QuarantineVesselModel)QuarantineCompanyModel.FromDsQuarantine(quarantine);
            //           vesselconfig.UseThisLevel = quarantine.UseThisLevel;

            return new QuarantineVesselModel
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
                UseThisLevel = quarantine.UseThisLevel
            };


//            return vesselconfig;
        }

    }
}