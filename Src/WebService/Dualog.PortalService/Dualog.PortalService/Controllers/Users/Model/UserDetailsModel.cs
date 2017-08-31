using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dualog.PortalService.Core.Validation;
using Dualog.Data.Oracle.Shore.Model;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.Swagger;
using Dualog.PortalService.Controllers.Permissions.Model;
using Dualog.PortalService.Core;
using System;

namespace Dualog.PortalService.Controllers.Users.Model
{
    public class UserDetailsModel : DataAnnotationsValidatable, IOperationFilter
    {
        public long Id { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "Email is required.")]
        [NoWhitespace]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(200, ErrorMessage = "Email cannot be longer than 200 characters.")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot ve longer than 100 characters.")]
        [NoWhitespace]
        public string Name { get; set; }

        public bool IsVesselUser { get; set; }

        [StringLength( 20, MinimumLength = 1, ErrorMessage = "Phone number must be between 1 and 20 characters long.")]
        [NoWhitespace]
        public string PhoneNumber { get; set; }

        [NoWhitespace]
        [StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters.")]
        public string Address { get; set; }

        public bool AddrCpttoaddrBook { get; set; }
        public bool ForwardCopy { get; set; }
        public string ForwardTo { get; set; }
        public bool HideInAddressBook { get; set; }
        public string MessageFormat { get; set; }



        public IEnumerable<UserGroupModel> UserGroups { get; set; }

        public IEnumerable<PermissionDetails> Permissions { get; set; }

        public static UserDetailsModel FromDsUser( DsUser user )
        {
            return new UserDetailsModel
            {
                Address = user.Address,
                Email = user.Email,
                Id = user.Id,
                IsVesselUser = user.VesselUser ?? false,
                Name = user.Name,
                PhoneNumber = user.PhoneNr,
                Permissions = from p in user.Permissions
                              select new PermissionDetails
                              {
                                  Name = p.Function.Name,
                                  AllowType = (AccessRights) p.AllowType
                              },
                UserGroups = from ug in user.UserGroups
                             select new UserGroupModel
                             {
                                 Name = ug.Name,
                                 Id = ug.Id,
                                 Description = ug.Description
                             }                
            };
        }


        void IOperationFilter.Apply( Operation operation, OperationFilterContext context )
        {
            var response = operation.Responses[ "200" ];
            response.Examples = new Dictionary<string, object>()
            {
                [ "application/json" ] = new
                {
                    Name = "The name",
                    Permissions = new
                    {
                        add = new
                        {
                            Name = "Name",
                            AllowType = 2
                        },
                        update = new
                        {
                            PermissionName = new
                            {
                                AllowType = 1
                            }
                        },
                        delete = new[] { "Permission1", "Permission2", "Permission3" },
                    }
                }
            };
        }

    }
}