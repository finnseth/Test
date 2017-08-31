using System;

namespace Dualog.PortalService.Core.Data
{
    public class JsonObjectGraphException : Exception
    {
        public JsonObjectGraphException(string message)
            : base(message)
        {
        }
    }
}
