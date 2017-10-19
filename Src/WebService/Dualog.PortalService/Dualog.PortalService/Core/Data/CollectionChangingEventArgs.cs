using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Dualog.PortalService.Core.Data
{
    public class CollectionChangingEventArgs : EventArgs
    {
        readonly string _path;
        readonly CollectionAction _action;
        object _value;
        readonly JObject _json;

        public CollectionChangingEventArgs( string path, CollectionAction action, object value, JObject json )
        {
            _json = json;
            _value = value;
            _action = action;
            _path = path;
        }

        public string Path => _path;

        public CollectionAction Action => _action;

        public object Value { get => _value; set => _value = value; }

        public JObject Json => _json;

        public Exception Exception { get; set; }
    }
}
