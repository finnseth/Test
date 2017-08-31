using Dualog.PortalService.Core.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dualog.Data.Oracle.Shore.Model;

namespace Dualog.PortalService.Controllers.Services.Model
{
    public class ServiceDetails : DataAnnotationsValidatable
    {
        public long Id { get; set; }

        [Required( AllowEmptyStrings = true, ErrorMessage = "Name is required." )]
        [StringLength( 80, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 80 characters long." )]
        [NoWhitespace]
        public string Name { get; set; }

        [Range( 0, 99 )]
        public int Protocol { get; set; }

        [Range( 0, ushort.MaxValue )]
        public int Port { get; set; }

        [Required( AllowEmptyStrings = true, ErrorMessage = "Ports is required." )]
        [StringLength( 255, MinimumLength = 1, ErrorMessage = "Ports must be between 1 and 255 characters long." )]
        [NoWhitespace]
        public string Ports { get; set; }


        public static ServiceDetails FromDsService( DsService service )
        {
            return new ServiceDetails {
                Id = service.Id,
                Name = service.Name,
                Port = service.Port,
                Ports = service.Ports,
                Protocol = service.Protocol
            };
        }
    }
}
