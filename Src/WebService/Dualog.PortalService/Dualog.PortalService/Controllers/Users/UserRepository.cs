﻿using System;
using System.Linq;
using Dualog.Data.Entity;
using Dualog.Data.Oracle.Shore.Model;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;
using Dualog.PortalService.Controllers.Users.Model;
using Dualog.Data.Oracle.Entity;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Dualog.PortalService.Core.Data;
using Microsoft.AspNetCore.Mvc;
using Dualog.PortalService.Controllers.Permissions;
using Dualog.PortalService.Core;

namespace Dualog.PortalService.Controllers.Users
{
    public class UserRepository
    {
        readonly IDataContextFactory _dcFactory;

        public UserRepository( IDataContextFactory dcFactory )
        {
            _dcFactory = dcFactory;
        }

        /// <summary>
        /// Get all users for a given company
        /// </summary>
        /// <param name="companyId">The id of the company.</param>
        /// <returns></returns>
        public async Task<IEnumerable<UserSummaryModel>> GetUsersAsync( long companyId )
        {
            using( var dc = _dcFactory.CreateContext() )
            {
                var q = from u in dc.GetSet<DsUser>()
                        where u.Company.Id == companyId
                        select new UserSummaryModel
                        {
                            Id = u.Id,
                            Email = u.Email,
                            Name = u.Name,
                            IsVesselUser = u.VesselUser ?? false,
                        };

                return await q.ToListAsync();
            }
        }

        /// <summary>
        /// Gets the specifier user asynchronously.
        /// </summary>
        /// <param name="id">The id of the user to get</param>
        /// <param name="companyId">The logged in users company id.</param>
        /// <returns></returns>
        public async Task<UserDetailsModel> GetUserDetailsAsync( long id, long companyId )
        {
            using( var dc = _dcFactory.CreateContext() )
            {
                var q = from u in dc.GetSet<DsUser>()
                        where u.Id == id && u.Company.Id == companyId
                        select new UserDetailsModel
                        {
                            Id = u.Id,
                            Email = u.Email,
                            Name = u.Name,
                            Address = u.Address,
                            PhoneNumber = u.PhoneNr,
                            IsVesselUser = u.VesselUser ?? false,
                            AddrCpttoaddrBook = u.AddrCpttoaddrBook,
                            ForwardCopy = u.ForwardCopy,
                            ForwardTo = u.ForwardTo,
                            HideInAddressBook = u.HideInAddressBook,
                            MessageFormat = u.MimeFormat ? "Mime" : "Dualog",
                            UserGroups = from ug in u.UserGroups
                                         select new UserGroupModel
                                         {
                                             Id = ug.Id,
                                             Name = ug.Name,
                                             Description = ug.Description
                                         },
                            //Permissions = from p in u.Permissions
                            //              select new Permission
                            //              {
                            //                  Name = p.Function.Name,
                            //                  AllowType = (short) p.AllowType
                            //              }
                        };

                var user = await q.FirstOrDefaultAsync();

                if( user == null )
                    throw new NotFoundException();


                var qPermissions = PermissionRepository.CreateRegularPermissionQuery( id, dc );
                user.Permissions = await qPermissions.ToListAsync();

                return user;
            }
        }


        public async Task<IEnumerable<UserGroupModel>> GetUserGroups( long companyId )
        {
            using( var dc = _dcFactory.CreateContext() )
            {
                var q = from ug in dc.GetSet<DsUserGroup>()
                        where ug.Company.Id == companyId
                        select new UserGroupModel
                        {
                            Name = ug.Name,
                            Description = ug.Description,
                            Id = ug.Id
                        };

                return await q.ToListAsync();
            }
        }


        public async Task CreateUser( UserDetailsModel user, long companyId )
        {
            using( var dc = _dcFactory.CreateContext() )
            {
                await InternalCreateUser( dc, user, companyId );
            }
        }

        public static async Task InternalCreateUser( IDataContext dc, UserDetailsModel userDetails, long companyId )
        {
            try
            {
                ICanCreateSequenceNumbers seq = dc as ICanCreateSequenceNumbers;
                userDetails.Id = await seq.GetSequenceNumberAsync<DsUser>();

                var newUser = dc.Add(new DsUser
                {
                    Id = userDetails.Id,
                    Language = 2, // English

                    Company = dc.Attach<DsCompany>( c => c.Id = companyId),
                    Name = userDetails.Name,
                    Address = userDetails.Address,
                    PhoneNr = userDetails.PhoneNumber,
                    CreatedAt = DateTime.UtcNow,
                    Email = userDetails.Email,

                });

                var sq = dc as ICanCreateSequenceNumbers;

                // Save the permissions
                if( userDetails.Permissions?.Any() == true )
                {

                    newUser.Permissions = new List<DsPermissionFunction>();

                    var functions = await dc.GetSet<DsFunction>()
                                            .Where( f => f.RowStatus == 0 && f.Id >= 1000 )
                                            .ToDictionaryAsync( k => k.Name, v => v );

                    int i = 0;
                    var sequenceNumbers = await sq.GetSequenceNumbersAsync<DsPermissionFunction>( userDetails.Permissions.Count() );
                    foreach( var permission in userDetails.Permissions )
                    {
                        if( functions.TryGetValue( permission.Name, out var function ) == false )
                            continue;


                        var entity = dc.Add( new DsPermissionFunction
                        {
                            Id = sequenceNumbers[i++],
                            AllowType = (int) permission.AllowType,
                            Function = function,
                            User = newUser
                        } );

                        newUser.Permissions.Add( entity );
                    }
                }


                await dc.SaveChangesAsync();

            }
            catch( Exception exception )
            {
                throw exception;
            }
        }



        public async static Task InternalGrantPermission( IDataContext dc, string permissionName, short allowType, long userId, long companyId )
        {
            try
            {
                var eq = dc as ICanCreateSequenceNumbers;

                var function = await dc.GetSet<DsFunction>()
                                .Where( f => f.Name.ToLower() == permissionName.ToLower() )
                                .Select( f => f.Id )
                                .FirstOrDefaultAsync();



                dc.Add( new DsPermissionFunction
                {
                    Id = await eq.GetSequenceNumberAsync<DsPermissionFunction>(),
                    AllowType = allowType,
                    Function = dc.Attach<DsFunction>( f => f.Id = function ),
                    User = dc.Attach<DsUser>( u => u.Id = userId ),
                } );

                await dc.SaveChangesAsync();
            }
            catch( Exception exception )
            {
                throw exception;
            }
        }

        public static async Task InternalRevokePermission( IDataContext dc, string permissionName, long userId, long companyId )
        {
            var function = await dc.GetSet<DsFunction>()
                                .Where( f => f.Name.ToLower() == permissionName )
                                .Select( f => f.Id )
                                .FirstOrDefaultAsync();

            var eq = dc as ICanExecuteQuery;

            var sql = @"DELETE FROM DS_PERMISSIONFUNCTION WHERE FUN_FUNCTIONID = :f AND USR_USERID = :u";
            await eq.ExecuteSqlCommandAsync( sql, function, userId );
        }

        public async Task DeleteAllUsersInCompany( long companyId )
        {
            using( var dc = _dcFactory.CreateContext() )
            {
                await InternalDeleteAllUsersInCompany( dc, companyId );
            }
        }

        public static async Task InternalDeleteAllUsersInCompany( IDataContext dc, long companyId )
        {
            var eq = dc as ICanExecuteQuery;

            string sql = @"DELETE FROM DS_USER WHERE COM_COMPANYID = :cmp";
            await eq?.ExecuteSqlCommandAsync( sql, companyId );
        }

        public async Task DeleteUser( long id, long companyId )
        {
            using( var dc = _dcFactory.CreateContext() )
            {
                var eq = dc as ICanExecuteQuery;

                string sql = @"UPDATE DS_USER 
                               SET USR_ROWSTATUS = 2 
                               WHERE COM_COMPANYID = :cmp AND USR_USERID = :usr";
                await eq?.ExecuteSqlCommandAsync( sql, companyId, id );
            }
        }


        public async Task<UserDetailsModel> PatchUserAsync( JObject json, long id )
        {
            using( var dc = _dcFactory.CreateContext() )
            {
                (dc as IHasChangeDetection)?.EnableChangeDetection();

                var user = dc.GetSet<DsUser>()
                                .Where( u => u.Id == id )
                                .Include( "UserGroups" )
                                .Include( "Permissions.Function" )
                                .FirstOrDefault();

                if( user == null )
                    throw new NotFoundException();


                var jog = new JsonObjectGraph(json, dc);

                jog.AddPropertyMap( "PhoneNumber", "PhoneNr" );
                jog.AddPropertyMap( "IsVesselUser", "VesselUser" );

                jog.LookupObjectById = ( path, value ) =>
                {
                    if( path == "/permissions/delete" || path == "/permissions/update" )
                    {
                        var pf = user.Permissions.FirstOrDefault( p => p.Function.Name.ToLower() == value.ToString().ToLower() );
                        if( pf == null )
                            throw new ValidationException( $"The permission with the name {value} was not found." );

                        return pf;
                    }
                    return null;
                };


                jog.CollectionChanging += async ( s, e ) =>
                {
                    var jItem = e.Json;

                    switch( e.Path )
                    {
                        case "/permissions":
                            string functionName = jItem.GetValue( "name", StringComparison.OrdinalIgnoreCase ).Value<string>()?.ToLower();

                            var function = await dc.GetSet<DsFunction>()
                                           .Where( pf => pf.Name.ToLower() == functionName )
                                           .FirstOrDefaultAsync();

                            if( function == null )
                            {
                                e.Exception = new ValidationException( $"The permission with name {functionName} does not exists." );
                                return;
                            }

                            if( function != null )
                            {
                                var permission = e.Value as DsPermissionFunction;
                                permission.Function = function;
                            }
                            break;
                    }

                };

                await jog.ApplyToAsync( user, new DefaultContractResolver() );

                var rUserDetails = UserDetailsModel.FromDsUser( user );


                await dc.SaveChangesAsync();
                return rUserDetails;
            }
        }
    }
}

