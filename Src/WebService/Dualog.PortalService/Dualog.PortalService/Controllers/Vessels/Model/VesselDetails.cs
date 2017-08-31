using Dualog.PortalService.Core.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Dualog.PortalService.Controllers.Vessels.Model
{
    public class VesselDetails : DataAnnotationsValidatable
    {
        int _category;

        public long Id { get; set; }

        [Required(AllowEmptyStrings = true, ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters long.")]
        [NoWhitespace]
        public string Name { get; set; }

        [Range(0, 99)]
        public int? Category
        {
            get { return _category; }
            set { _category = value ?? 0; }
        }

        [Range(1, 10)]
        public short AccountEnabled { get; set; }

        [StringLength(20, MinimumLength = 1, ErrorMessage = "IMO must be between 1 and 20 characters long.")]
        [NoWhitespace]
        public string IMO { get; set; }

    }
}