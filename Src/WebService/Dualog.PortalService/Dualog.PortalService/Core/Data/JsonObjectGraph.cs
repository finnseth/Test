using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dualog.Data.Entity;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Dualog.PortalService.Core.Data
{
    public class JsonObjectGraph
    {
        JObject _json;
        IDataContext _dataContext;
        Dictionary<string, string> _propertyNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        Dictionary<string, Func<string, object, Task<object>>> _propertyChangeHandlers = new Dictionary<string, Func<string, object, Task<object>>>(StringComparer.OrdinalIgnoreCase);
        List<string> _ignoreProperties = new List<string>();


        public event EventHandler<CollectionChangingEventArgs> CollectionChanging;
        public Func<string, object, object> LookupObjectById { get; set; }



        public JsonObjectGraph( JObject json )
        {
            _json = json;
        }

        public JsonObjectGraph( JObject json, IDataContext dataContext )
            : this( json )
        {
            _dataContext = dataContext;
            (_dataContext as IHasChangeDetection)?.EnableChangeDetection();
        }

        /// <summary>
        /// Adds a path to a property to be ignored.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public JsonObjectGraph IgnoreProperty( string path )
        {
            _ignoreProperties.Add( path );

            return this;
        }


        /// <summary>
        /// Adds a new map entry t
        /// </summary>
        /// <param name="path"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public JsonObjectGraph AddPropertyMap( string path, string propertyName )
        {
            _propertyNames[ path ] = propertyName;

            return this;
        }


        /// <summary>
        /// Adds a method to be called when a property's value is about to be set.
        /// </summary>
        /// <param name="path">The property path which uniquely identifies the property in the object graph.</param>
        /// <param name="handler">The handler to be called.</param>
        /// <returns></returns>
        public JsonObjectGraph AddPropertyChangeHandler( string path, Func<string, object, Task<object>> handler )
        {
            _propertyChangeHandlers[ path ] = handler;

            return this;
        }

        public async Task ApplyToAsync<T>( T entity, IContractResolver contractResolver ) where T : IEntity
        {
            await ParseObjectAsync( entity, _json, contractResolver );
        }


        private async Task ParseObjectAsync( IEntity entity, JObject obj, IContractResolver contractResolver )
        {
            Type type = entity.GetType();

            foreach( var property in obj.Properties() )
            {
                // Check whether to property is in the ignore-list, and skip it if it does.
                if( _ignoreProperties.Contains( GetPropertyPath( property.Path ), StringComparer.OrdinalIgnoreCase ) )
                    continue;

                var propertyName = GetPropertyName(property.Name);

                var pi = type.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                if( pi == null )
                    continue;    // throw new JsonObjectGraphException($"Unknown property {propertyName} on type {type.Name}.");


                if( pi.PropertyType.GetInterface( "ICollection" ) != null )
                {
                    await ParseCollectionObjectAsync( entity, pi, property, contractResolver );
                }


                else if( property.FirstOrDefault()?.Type == JTokenType.Object )
                {
                    var o = pi.GetValue(entity) as IEntity;
                    if( o == null )
                    {
                        var contract = contractResolver.ResolveContract(pi.PropertyType);
                        o = contract.DefaultCreator() as IEntity;
                    }

                    await ParseObjectAsync( o, property.Value as JObject, contractResolver );

                    o = await CheckPropertyChangeHandlerAsync( o, property.Path ) as IEntity;
                    pi.SetValue( entity, o );
                }

                else
                {
                    if( property.Value.Type == JTokenType.Null )
                        pi.SetValue( entity, null );

                    else
                    {
                        object v = null;

                        try
                        {
                            v = property.Value.ToObject( pi.PropertyType );
                            v = await CheckPropertyChangeHandlerAsync( v, property.Path );
                        }
                        catch
                        {
                            v = property.Value.ToObject( typeof( object ) );
                            v = await CheckPropertyChangeHandlerAsync( v, property.Path );

                            ChangeType( v, pi.PropertyType );
                        }


                        pi.SetValue( entity, v );
                    }
                    continue;
                }
            }
        }

        private async Task ParseCollectionObjectAsync( IEntity entity, PropertyInfo propertyInfo, JToken obj, IContractResolver contractResolver )
        {
            var co = obj.FirstOrDefault() as JObject;
            if( co == null )
                return;

            // if the collection is null, create a new collection and assign it.
            if( propertyInfo.GetValue( entity ) == null )
            {
                var cr = contractResolver.ResolveContract(propertyInfo.PropertyType);
                object v = cr.DefaultCreator();
                propertyInfo.SetValue( entity, v );
            }

            var list = propertyInfo.GetValue(entity) as IList;


            foreach( var prop in co.Properties() )
            {
                switch( prop.Name )
                {
                    case "add":
                        await AddObjectFromArrayAsync( prop, list, contractResolver );
                        break;

                    case "update":
                        await UpdateObjectsFromArrayAsync( prop, list, contractResolver );
                        break;

                    case "delete":
                        RemoveObjectRefFromArray( prop, list, contractResolver );
                        break;

                    default:
                        throw new JsonObjectGraphException( $"Unknown operation '{prop.Name}'. Available operations are: add, update, delete." );
                }
            }
        }

        async Task AddObjectFromArrayAsync( JProperty property, IList list, IContractResolver contractResolver )
        {
            JArray jArray = DoArrayTypeCheck(property.FirstOrDefault());
            var listType = InferListType(list);
            var collectionTypeContract = contractResolver.ResolveContract(listType);

            foreach( JObject item in jArray )
            {

                var idProp = item.GetValue( "id", StringComparison.OrdinalIgnoreCase )?.Value<long>();
                if( idProp != null )
                {
                    AddReferenceObjects( idProp.Value, list, contractResolver );
                    continue;
                }


                var entity = collectionTypeContract.DefaultCreator() as IEntity;
                await ParseObjectAsync( entity, item, contractResolver );

                // Get and assign a new sequence number
                if( _dataContext != null )
                {
                    entity.Id = await _dataContext.GetSequenceNumberAsync( entity.GetType() );
                }



                string path = GetPropertyPath( RemoveCollectionSuffix( property.Path ) );
                FireCollectionChangingEvent( path, CollectionAction.Add, entity, item );

                list.Add( entity );
            }
        }

        private static string RemoveCollectionSuffix( string path )
        {
            int index = path.LastIndexOf( '.' );
            if( index < 0 )
                return path;

            return path.Substring( 0, index );
        }


        void RemoveObjectRefFromArray( JProperty property, IList list, IContractResolver contractResolver )
        {
            var jArray = DoArrayTypeCheck(property.FirstOrDefault());
            var listType = InferListType(list);

            const string name = nameof(IEntity);
            if( listType.GetInterface( name ) == null )
                throw new JsonObjectGraphException( $"Items in collection to delete from must implement {name}" );

            var collectionTypeContract = contractResolver.ResolveContract(listType);

            foreach( var item in jArray.Children() )
            {
                var val = (item as JValue)?.Value;

                var entity = FindItemById(list, val, "id") as IEntity;
                if( entity == null && LookupObjectById != null )
                    entity = LookupObjectById( GetPropertyPath( property.Path ), val ) as IEntity;


                // Attach the entity
                if(entity != null && _dataContext != null )
                {
                    // Get the dataset by reflection
                    var getSetMethod = typeof(IDataContext).GetMethod(nameof(IDataContext.GetSet)).MakeGenericMethod(listType);
                    var ds = getSetMethod.Invoke(_dataContext, null);

                    getSetMethod = ds.GetType().GetMethod( "Remove" );
                    getSetMethod.Invoke( ds, new[] { entity } );
                }
                else if( entity != null && _dataContext == null)
                {
                    list.Remove( entity );
                }
            }
        }

        async Task UpdateObjectsFromArrayAsync( JProperty property, IList list, IContractResolver contractResolver )
        {
            var listType = InferListType(list);
            var collectionTypeContract = contractResolver.ResolveContract(listType);

            const string name = nameof(IEntity);
            if( listType.GetInterface( name ) == null )
                throw new JsonObjectGraphException( $"Items in collection to update must implement {name}" );

            var updateObject = property.SingleOrDefault(o => o.Type == JTokenType.Object);

            foreach( var item in updateObject.Children().OfType<JProperty>().Cast<JProperty>() )
            {
                object id = item.Name;
                var found = FindItemById(list, id, "id");
                if( found == null && LookupObjectById != null )
                    found = LookupObjectById( GetPropertyPath( property.Path ), item.Name );

                if( found != null )
                    await ParseObjectAsync( found as IEntity, item.First() as JObject, contractResolver );
            }
        }

        void AddReferenceObjects( long id, IList list, IContractResolver contractResolver )
        {
            var listType = InferListType(list);

            const string name = nameof(IEntity);
            if( listType.GetInterface( name ) == null )
                throw new JsonObjectGraphException( $"Items in collection to delete from must implement {name}" );

            var collectionTypeContract = contractResolver.ResolveContract(listType);

            var entity = collectionTypeContract.DefaultCreator() as IEntity;
            entity.Id = id;


            // Attach the entity
            if( _dataContext != null )
            {
                // Get the dataset by reflection
                var getSetMethod = typeof(IDataContext).GetMethod(nameof(IDataContext.GetSet)).MakeGenericMethod(listType);
                var ds = getSetMethod.Invoke(_dataContext, null);

                getSetMethod = ds.GetType().GetMethod( "Add" );
                getSetMethod.Invoke( ds, new[] { entity } );
            }
        }



        private static JArray DoArrayTypeCheck( JToken token )
        {
            if( token.Type != JTokenType.Array )
                throw new JsonObjectGraphException( "Expected a Json array." );

            return token as JArray;
        }

        private static Type InferListType( IList list )
        {
            // Try to infer the type of collection
            var ct = list.GetType().GetGenericArguments().FirstOrDefault();
            if( ct == null )
                throw new JsonObjectGraphException( "Unable to infer type from collection. Type must be a generic List<T>." );

            return ct;
        }

        private static object FindItemById( IList list, object id, string idProp )
        {
            object found = null;

            foreach( var item in list )
            {
                var pi = item.GetType().GetProperty( idProp, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public );
                if( pi == null )
                    break;

                object value = null;

                try
                {
                    value = pi.GetValue( item );
                    id = ChangeType( id, value.GetType() );
                }
                catch
                {
                    break;
                }


                if( value.Equals( id ) )
                {
                    found = item as IEntity;
                    break;
                }
            }

            return found;
        }

        private static object ChangeType( object value, Type conversion )
        {
            var t = conversion;

            if( t.IsGenericType && t.GetGenericTypeDefinition().Equals( typeof( Nullable<> ) ) )
            {
                if( value == null )
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType( t );
            }

            if( t != value.GetType() )
                return Convert.ChangeType( value, t );
            else
                return value;
        }



        private string GetPropertyName( string propertyName )
        {
            if( _propertyNames != null && _propertyNames.TryGetValue( propertyName, out var name ) )
                return name;

            return propertyName;
        }

        private async Task<object> CheckPropertyChangeHandlerAsync( object entity, string path )
        {
            string fixedPath = GetPropertyPath(path);

            if( _propertyChangeHandlers.TryGetValue( fixedPath, out var handler ) )
                return await handler( fixedPath, entity );

            return entity;
        }

        private static string GetPropertyPath( string path )
        {
            string fixedPath = string.Empty;
            if( path.StartsWith( "/" ) == false )
                fixedPath += '/';

            fixedPath += path.Replace( '.', '/' );
            return fixedPath;
        }

        void FireCollectionChangingEvent( string path, CollectionAction action, object value, JObject json )
        {
            var eventArgs = new CollectionChangingEventArgs( path, action, value, json );
            CollectionChanging?.Invoke( this, eventArgs );

            if( eventArgs.Exception != null )
                throw eventArgs.Exception;
        }
    }
}
