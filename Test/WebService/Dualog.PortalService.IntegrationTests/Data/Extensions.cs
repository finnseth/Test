using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dualog.PortalService.Data
{
    internal static class Extensions
    {
        public static StringBuilder ConcatToStringBuilder( this Stack<char> stack )
        {
            var sb = stack.Aggregate( new StringBuilder(), ( a, b ) => a.Append( b ) );
            stack.Clear();

            return sb;
        }
    }
}
