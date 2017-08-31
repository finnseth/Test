using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dualog.PortalService.Core.Validation;
using Dualog.Data.Oracle.Common.Model;

namespace Dualog.PortalService.Controllers.Dashboard
{
    public class Dashboard : DataAnnotationsValidatable
    {
        public long Id { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "Name is required." )]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters long.")]
        [NoWhitespace]
        public string Name { get; set; }

        public static Dashboard FromApDashboardInstance(ApDashboardInstance entity)
        {
            return new Dashboard
            {
                Id = entity.Id,
                Name = entity.Name
            };

        }

    }
}
