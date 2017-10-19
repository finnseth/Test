using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dualog.PortalService.Core.Validation
{
    public interface IValidatable
    {
        bool Validate( out string message );
    }
}
