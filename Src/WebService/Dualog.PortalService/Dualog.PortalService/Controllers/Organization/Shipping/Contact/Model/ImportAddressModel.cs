using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Dualog.PortalService.Core.Validation;
using Dualog.Data.Oracle.Shore.Model;

namespace Dualog.PortalService.Controllers.Organization.Shipping.Contact.Model
{
    public class ImportAddressModel : DataAnnotationsValidatable
    {

        public long Id { get; set; }

        public long Company { get; set; }

        public long? Ship { get; set; }

        public long? User { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "Recipient is required.")]
        [NoWhitespace]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(300, ErrorMessage = "Email cannot be longer than 300 characters.")]
        public string Recipient { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "Return recipient is required.")]
        [NoWhitespace]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(300, ErrorMessage = "Email cannot be longer than 300 characters.")]
        public string ReturnRecipient { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "Sender is required.")]
        [NoWhitespace]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(300, ErrorMessage = "Email cannot be longer than 300 characters.")]
        public string Sender { get; set; }


        public static ImportAddressModel FromDsImportAddress(DsImportAddress importaddress)
        {
            return new ImportAddressModel
            {
                Id = importaddress.Id,
                Recipient = importaddress.Recipient,
                ReturnRecipient = importaddress.ReturnRecipient,
                Sender = importaddress.Sender,
                Company = importaddress.Company.Id,
                Ship = importaddress.Vessel.Id,
                User = importaddress.User.Id

            };
        }
    }
}
