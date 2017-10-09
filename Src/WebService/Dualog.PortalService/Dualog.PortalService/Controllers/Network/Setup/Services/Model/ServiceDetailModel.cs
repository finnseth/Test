using Dualog.PortalService.Core.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dualog.Data.Oracle.Shore.Model;

namespace Dualog.PortalService.Controllers.Network.Setup.Services.Model
{
    public class ServiceDetailModel : DataAnnotationsValidatable
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

        // Commented this out due to integration tests failing
        // public int BelongsTo { get; set; }

        public string Company { get; set; }

        public string Ship { get; set; }

        public static ServiceDetailModel FromDsService( DsService service )
        {
            return new ServiceDetailModel {
                Id = service.Id,
                Name = service.Name,
                Port = service.Port,
                Ports = service.Ports,
                Protocol = service.Protocol,
                
            };
        }
    }
}
