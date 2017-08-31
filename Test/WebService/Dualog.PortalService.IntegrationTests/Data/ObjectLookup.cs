using System;
using System.Collections.Generic;
using System.Linq;

namespace Dualog.PortalService.Data
{
    public class ObjectLookup
    {
        Dictionary<string, object> _objectLookup;

        public ObjectLookup( Dictionary<string, object> dictionary )
        {
            _objectLookup = dictionary;
        }

        public T GetObjectById<T>( string id ) where T : class
        {
            if( _objectLookup.ContainsKey( id ) == true )
                return _objectLookup[ id ] as T;
            else
                return default( T );
        }

        public object GetObjectById( string id )
        {
            if( _objectLookup.ContainsKey( id ) == true )
                return _objectLookup[ id ];
            else
                return null;

        }

    }
}
