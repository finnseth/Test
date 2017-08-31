using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dualog.Data.Oracle.Shore.Model;
using Dualog.PortalService.Controllers.Permissions.Model;
using Dualog.PortalService.Core;
using Dualog.PortalService.Core.Validation;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Dualog.PortalService.Controllers.UserGroups.Model
{
    public class UserGroupDetails : DataAnnotationsValidatable, IOperationFilter
    {
        public long Id { get; set; }

        [Required( AllowEmptyStrings = true, ErrorMessage = "Name is required." )]
        [StringLength( 100, ErrorMessage = "Name cannot ve longer than 100 characters." )]
        [NoWhitespace]
        public string Name { get; set; }

        [StringLength( 300, ErrorMessage = "Description cannot ve longer than 300 characters." )]
        [NoWhitespace]
        public string Description { get; set; }

        public bool NeedApproval { get; set; }
            
        public bool AllowFax { get; set; }

        public bool FaxDeliveryReport { get; set; }

        public bool FaxNotDeliveredReport { get; set; }

        public bool AllowTelex { get; set; }

        public bool TelexDeliveryReport { get; set; }

        public bool TelexNotDeliveredReport { get; set; }

        public bool AllowSms { get; set; }

        public bool SmsDeliveryReport { get; set; }

        public bool SmsNotDeliveredReport { get; set; }

        public bool UseImap { get; set; }

        public bool UsePop { get; set; }

        [Range(0, 99, ErrorMessage = "AttachmentRule must be between 0 and 99." )]
        public short AttachmentRule { get; set; }

        [Range( 0, 99, ErrorMessage = "DaysAutoSignOff must be between 0 and 99." )]
        public short DaysAutoSignOff { get; set; }

        public bool DeleteOldMessages { get; set; }

        [Range( 0, 1440, ErrorMessage = "DaysAutoSignOff must be between 0 and 1440." )]
        public int MinutesLoginPerDay { get; set; }

        [Range( 0, 999, ErrorMessage = "ConcurrentDevices must be between 0 and 999." )]
        public int ConcurrentDevices { get; set; }

        public bool SigninApproval { get; set; }

        [Range( 0, 9999, ErrorMessage = "DaysToKeepMessages must be between 0 and 9999." )]
        public int DaysToKeepMessages { get; set; }

        [Range( 0, 99999999, ErrorMessage = "IpTimeout must be between 0 and 99999999." )]
        public int IpTimeout { get; set; }

        public bool SmtpRelay { get; set; }

        public IEnumerable<PermissionDetails> Permissions { get; set; }

        public static UserGroupDetails FromDsUSerGroup( DsUserGroup userGroup )
        {
            var newUserGroupDetails = new UserGroupDetails()
            {
                AllowFax = userGroup.AllowFax ?? false,
                AllowSms = userGroup.AllowSms ?? false,
                AllowTelex = userGroup.AllowTelex ?? false,
                AttachmentRule = userGroup.AttachmentRule ?? 0,
                ConcurrentDevices = userGroup.ConcurrentDevices ?? 0,
                DaysAutoSignOff = userGroup.DaysAutoSignOff ?? 0,
                DaysToKeepMessages = userGroup.DaysToKeepMessages ?? 0,
                DeleteOldMessages = userGroup.DeleteOldMessages ?? false,
                Description = userGroup.Description,
                FaxDeliveryReport = userGroup.FaxDr ?? false,
                FaxNotDeliveredReport = userGroup.FaxNdr ?? false,
                Id = userGroup.Id,
                IpTimeout = userGroup.IpTimeout ?? 0,
                MinutesLoginPerDay = userGroup.MinutesLoginPerDay ?? 0,
                Name = userGroup.Name,
                NeedApproval = userGroup.NeedsApproval ?? false,
                SigninApproval = userGroup.SigninApproval ?? false,
                SmsDeliveryReport = userGroup.SmsDr ?? false,
                SmsNotDeliveredReport = userGroup.SmsNdr ?? false,
                SmtpRelay = userGroup.SmtpRelay ?? false,
                TelexDeliveryReport = userGroup.TelexDr ?? false,
                TelexNotDeliveredReport = userGroup.TelexNdr ?? false,
                UseImap = userGroup.Imap ?? false,
                UsePop = userGroup.Pop ?? false,
                Permissions = userGroup.Permissions
                                       .Where( p => p.AllowType > 0 )
                                       .Select( p => new PermissionDetails
                                       {
                                           AllowType = (AccessRights) p.AllowType,
                                           Name = p.Function.Name
                                       } )
            };
            return newUserGroupDetails;
        }

        void IOperationFilter.Apply( Operation operation, OperationFilterContext context )
        {
            var response = operation.Responses[ "200" ];
            response.Examples = new Dictionary<string, object>()
            {
                [ "application/json" ] = new {
                    Name = "The name",
                    Permissions = new {
                        add = new {
                            Name = "PermissionName",
                            AllowType = 2
                        },
                        update = new
                        {
                            PermissionName = new
                            {
                                AllowType = AccessRights.Read
                            }
                        },
                        delete = new[] { "Permission1", "Permission2", "Permission3" },
                    }
                }
            };
        }
    }
}
