using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Dualog.PortalService.Core.Validation;
using Dualog.Data.Oracle.Shore.Model;
using Newtonsoft.Json;
using NJsonSchema.Converters;
using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace Dualog.PortalService.Controllers.Organization.Shipping.Contact.Model
{
    [JsonConverter(typeof(JsonInheritanceConverter), "AddressModel")]
    [KnownType(typeof(AddressModel))]
    public class AddressModel : DataAnnotationsValidatable
    {

        public long Id { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "Email is required.")]
        [NoWhitespace]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(200, ErrorMessage = "Email cannot be longer than 200 characters.")]
        public string Email { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public AddressImportStatus Status { get; set; }

        public string FirstName { get; set; }

        public string SurName { get; set; }

        [StringLength(20, ErrorMessage = "PhoneNumber cannot be longer than 20 characters.")]
        public string Phone { get; set; }

        public IEnumerable<AddressFieldModel> Fields { get; set; }

        public IEnumerable<AddressGroupModel> AddressGroup { get; set; }


        public static AddressModel FromDsAddress(DsAddress address)
        {
            return new AddressModel
            {
                Id = address.Id,                
                Name = address.Name,
                FirstName = address.FirstName,
                SurName = address.SurName,
                Email = address.Email,
                Phone = address.PhoneNumber,
                Status = address.Status,
                Fields = from fields in address.Fields
                         where address.Id == fields.Address.Id
                         select new AddressFieldModel
                         {
                             Id = fields.Id,
                             Field = fields.Field.Id,
                             Value = fields.Value
                         },
                AddressGroup = from g in address.AddressGroup
                                      select new AddressGroupModel
                                      {
                                          Id = g.Id,
                                          Name = g.Name
                                      }

            };
        }
    }
}
