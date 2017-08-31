using System;
using System.Linq;

namespace Dualog.PortalService.Core
{
    public enum AccessRights
    {
        Read = 1,
        Write = 2
    }

    public enum ShipCategory
    {
        Ordinary = 0,
        Trial = 1,
        IFU = 2,
        Test = 3,
        ShipTrial = 4,
        OfficeTrial = 5            
    }
}
