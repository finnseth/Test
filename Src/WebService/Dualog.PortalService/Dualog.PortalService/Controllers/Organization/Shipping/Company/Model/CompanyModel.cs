using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Dualog.PortalService.Core.Validation;
using Dualog.Data.Oracle.Shore.Model;

namespace Dualog.PortalService.Controllers.Organization.Shipping.Company.Model
{
    public class CompanyModel : DataAnnotationsValidatable
    {

        public long Id { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [StringLength(100, ErrorMessage = "Address cannot be longer than 100 characters.")]
        public string Address { get; set; }

        [StringLength(20, ErrorMessage = "PhoneNumber cannot be longer than 20 characters.")]
        public string Phone { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "Email is required.")]
        [NoWhitespace]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(200, ErrorMessage = "Email cannot be longer than 200 characters.")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "Manager is required.")]
        [StringLength(100, ErrorMessage = "Manager cannot be longer than 100 characters.")]
        public string Manager { get; set; }

        // todo: how to know that this field is only allowed to change if you are a Admin
        public long? CustomerNumber { get; set; }

        
        public static CompanyModel FromDsCompany(DsCompany company)
        {
            return new CompanyModel
            {

                Id = company.Id,
                Name = company.Name,
                Address = company.Address,
                Phone = company.PhoneNumber,
                Email = company.Email,
                Manager = company.Manager,
                CustomerNumber = (long) company.CustomerNumber
            };
        }
    }
}
