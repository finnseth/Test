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
    [JsonConverter(typeof(JsonInheritanceConverter), "AddressGroupModel")]
    [KnownType(typeof(AddressGroupModel))]
    public class AddressGroupModel : DataAnnotationsValidatable
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<AddressModel> Address { get; set; }

        public static AddressGroupModel FromDsAddressGroup(DsAddressGroup addressgroup)
        {
            return new AddressGroupModel
            {
                Id = addressgroup.Id,
                Name = addressgroup.Name,
                Address = from g in addressgroup.Address
                          select new AddressModel
                          {
                              Id = g.Id,
                              Name = g.Name,
                              Email = g.Email
                          }
            };
        }
    }
}
