using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Dualog.Data.Oracle.Shore.Model;
using Dualog.PortalService.Controllers.Permissions.Model;
using Dualog.PortalService.Controllers.UserGroups.Model;
using Dualog.PortalService.Core;
using Dualog.PortalService.Core.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Dualog.PortalService.Controllers.UserGroups
{
    public class UserGroupsRepository
    {
        IDataContextFactory _dcFactory;

        public UserGroupsRepository( IDataContextFactory dcFactory )
        {
            _dcFactory = dcFactory;
        }

        public async Task<IEnumerable<UserGroupDetails>> GetUserGroups( long companyId, long? vesselId = null, long? userGroupId = null )
        {
            using( var dc = _dcFactory.CreateContext() )
            {
                var q = dc.GetSet<DsUserGroup>().Where( ug => ug.Company.Id == companyId );
                if( vesselId != null )
                    q = q.Where( ug => ug.Vessel.Id == vesselId );

                if( userGroupId != null )
                    q = q.Where( ug => ug.Id == userGroupId );

                var result = from ug in q
                             select new UserGroupDetails
                             {
                                 Id = ug.Id,
                                 AllowFax = ug.AllowFax ?? false,
                                 AllowSms = ug.AllowSms ?? false,
                                 AllowTelex = ug.AllowTelex ?? false,
                                 AttachmentRule = ug.AttachmentRule ?? 0,
                                 ConcurrentDevices = ug.ConcurrentDevices ?? 0,
                                 DaysAutoSignOff = ug.DaysAutoSignOff ?? 0,
                                 DeleteOldMessages = ug.DeleteOldMessages ?? false,
                                 Description = ug.Description,
                                 FaxDeliveryReport = ug.FaxDr ?? false,
                                 FaxNotDeliveredReport = ug.FaxNdr ?? false,
                                 MinutesLoginPerDay = ug.MinutesLoginPerDay ?? 0,
                                 Name = ug.Name,
                                 NeedApproval = ug.NeedsApproval ?? false,
                                 SigninApproval = ug.SigninApproval ?? false,
                                 SmsDeliveryReport = ug.SmsDr ?? false,
                                 SmsNotDeliveredReport = ug.SmsNdr ?? false,
                                 TelexDeliveryReport = ug.TelexDr ?? false,
                                 TelexNotDeliveredReport = ug.TelexNdr ?? false,
                                 UseImap = ug.Imap ?? false,
                                 UsePop = ug.Pop ?? false,
                                 DaysToKeepMessages = ug.DaysToKeepMessages ?? 0,
                                 IpTimeout = ug.IpTimeout ?? 0,
                                 SmtpRelay = ug.SmtpRelay ?? false,
                                 Permissions = from p in ug.Permissions
                                               where p.AllowType > 0
                                               select new PermissionDetails
                                               {
                                                   AllowType = (AccessRights) p.AllowType,
                                                   Name = p.Function.Name
                                               }
                             };

                return await result.ToListAsync();
            }
        }

        public async Task AddUserGroup( UserGroupDetails userGroup, long companyId, long? vesselId = null )
        {
            using( var dc = _dcFactory.CreateContext() )
            using( var transaction = dc.BeginTransaction() )
            {
                (dc as IHasChangeDetection)?.EnableChangeDetection();
                var seq = dc as ICanCreateSequenceNumbers;

                var nUserGroup = dc.Add<DsUserGroup>( async ug =>
                {

                    ug.Id = await seq.GetSequenceNumberAsync<DsUserGroup>();
                    ug.AllowFax = userGroup.AllowFax;
                    ug.AllowSms = userGroup.AllowSms;
                    ug.AllowTelex = userGroup.AllowTelex;
                    ug.AttachmentRule = userGroup.AttachmentRule;
                    ug.Company = dc.Attach<DsCompany>( c => c.Id = companyId );
                    ug.ConcurrentDevices = userGroup.ConcurrentDevices;
                    ug.DaysAutoSignOff = userGroup.DaysAutoSignOff;
                    ug.DaysToKeepMessages = userGroup.DaysToKeepMessages;
                    ug.DeleteOldMessages = userGroup.DeleteOldMessages;
                    ug.Description = userGroup.Description;
                    ug.FaxDr = userGroup.FaxDeliveryReport;
                    ug.FaxNdr = userGroup.FaxNotDeliveredReport;
                    ug.Imap = userGroup.UseImap;
                    ug.IpTimeout = userGroup.IpTimeout;
                    ug.MinutesLoginPerDay = userGroup.MinutesLoginPerDay;
                    ug.Name = userGroup.Name;
                    ug.NeedsApproval = userGroup.NeedApproval;
                    ug.Pop = userGroup.UsePop;
                    ug.SigninApproval = userGroup.SigninApproval;
                    ug.SmsDr = userGroup.SmsDeliveryReport;
                    ug.SmsNdr = userGroup.SmsNotDeliveredReport;
                    ug.SmtpRelay = userGroup.SmtpRelay;
                    ug.TelexDr = userGroup.TelexDeliveryReport;
                    ug.TelexNdr = userGroup.TelexNotDeliveredReport;
                    ug.Vessel = vesselId != null ? dc.Attach<DsVessel>( v => v.Id = vesselId.Value ) : null;
                    ug.Permissions = new List<DsPermissionFunction>();
                } );

                userGroup.Id = nUserGroup.Id;

                await dc.SaveChangesAsync();


                if( userGroup.Permissions != null && userGroup.Permissions.Any() )
                {
                    var permissionNames = userGroup.Permissions
                                                   .Select( p => p.Name.ToLower() )
                                                   .ToArray();

                    var functions = await dc.GetSet<DsPermissionFunction>()
                                            .Include( pf => pf.Function )
                                            .Where( pf => pf.UserGroup.Id == userGroup.Id )
                                            .Where(f => permissionNames.Contains( f.Function.Name.ToLower() ) )
                                            .ToDictionaryAsync( k => k.Function.Name.ToLower() );



                    foreach( var permission in userGroup.Permissions )
                    {
                        if( functions.TryGetValue( permission.Name.ToLower(), out var func ) == false )
                        {
                            transaction.Rollback();
                            throw new ValidationException( $"The permission with name {permission.Name} does not exists." );
                        }

                        func.AllowType = (int) permission.AllowType;
                    }
                }

                transaction.Commit();
                await dc.SaveChangesAsync();
            }
        }


        public async Task<UserGroupDetails> ChangeUserGroupAsync( long id, JObject patch )
        {
            using( var dc = _dcFactory.CreateContext() )
            {
                (dc as IHasChangeDetection)?.EnableChangeDetection();

                var userGroup = dc.GetSet<DsUserGroup>()
                                  .Include( "Permissions" )
                                  .Include( "Permissions.Function")
                                  .Where( ug => ug.Id == id )
                                  .FirstOrDefault();

                if( userGroup == null )
                    throw new NotFoundException();


                var jog = new JsonObjectGraph( patch, dc );


                // Setting up property mapping
                jog.AddPropertyMap( "/useImap", "Imap" )
                   .AddPropertyMap( "/usePop", "Pop" )
                   .AddPropertyMap( "/smsNotDeliveredReport", "SmsNdr" )
                   .AddPropertyMap( "/smsDeliveryReport", "SmsDr" )
                   .AddPropertyMap( "/telexNotDeliveredReport", "TelexNdr" )
                   .AddPropertyMap( "/telexDeliveryReport", "TelexDr" )
                   .AddPropertyMap( "/faxNotDeliveredReport", "FaxNdr" )
                   .AddPropertyMap( "/faxDeliveryReport", "FaxDr" );


                jog.LookupObjectById = ( path, value ) =>
                {
                    if( path == "/permissions/update" )
                    {
                        var permission = userGroup.Permissions.FirstOrDefault( p => p.Function.Name.ToLower() == value.ToString().ToLower() );
                        if( permission == null )
                            throw new ValidationException( $"Permission with the name {value.ToString()} was not found." );

                        return permission;
                    }

                    return null;
                };


                await jog.ApplyToAsync( userGroup, new DefaultContractResolver() );

                var ugDetails = UserGroupDetails.FromDsUSerGroup( userGroup );
                if( ugDetails.Validate( out var message ) == false )
                    throw new ValidationException( message );


                await dc.SaveChangesAsync();


                return ugDetails;
            }
        }

        public async Task DeleteUserGroup( long companyId, long userGroupId )
        {
            using( var dc = _dcFactory.CreateContext() )
            {
                var eq = dc as ICanExecuteQuery;

                var sql = @"UPDATE DS_USERGROUP 
                            SET USG_ROWSTATUS = 2
                            WHERE COM_COMPANYID = :cmp AND USG_USERGROUPID = :ugid";
                await eq.ExecuteSqlCommandAsync( sql, companyId, userGroupId );
            }
        }


        public static async Task InternalDeleteUserGroupsForCompany( IDataContext dataContext, long companyId )
        {
            var eq = dataContext as ICanExecuteQuery;

            var sql = @"DELETE FROM DS_USERGROUP WHERE COM_COMPANYID = :cmp";

            await eq.ExecuteSqlCommandAsync( sql, companyId );
        }
    }
}
