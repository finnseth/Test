using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Dualog.PortalService.Data
{
    public class ValueTokenizer
    {
        int _position = 0;


        public IEnumerable<Token> Tokenize( string value )
        {
            var stack = new Stack<char>();

            var tokenType = TokenType.String;
            var sb = new StringBuilder();

            char lastChar = '\0';
            char currentChar = '\0';

            for( _position = 0; _position < value.Length; _position++ )
            {
                lastChar = currentChar;
                currentChar = value[ _position ];


                if( currentChar == '{' || currentChar == '@' || currentChar == '}' )
                    stack.Push( currentChar );
                else
                    sb.Append( currentChar );


                if( lastChar == '{' && currentChar == '@'  )
                {
                    if( sb.Length > 0 )
                    {
                        yield return new Token( TokenType.String, sb.ToString() );
                        sb.Clear();
                    }


                    tokenType = TokenType.ObjectRef;
                    continue;
                }

                if( currentChar == '}' && tokenType == TokenType.ObjectRef )
                {
                    yield return new Token( tokenType, sb.ToString() );

                    sb.Clear();
                    stack.Clear();
                    tokenType = TokenType.String;

                    continue;
                }




                if( _position == value.Length - 1 )
                {
                    sb.Append( stack.ConcatToStringBuilder().ToString() );

                    yield return new Token( tokenType, sb.ToString() );
                    sb.Clear();
                    continue;

                }
            }
        }


        public string Parse( IEnumerable<Token> tokens, ObjectLookup lookup )
        {
            StringBuilder sb = new StringBuilder();

            foreach( var item in tokens )
            {
                if( item.TokenType == TokenType.String )
                    sb.Append( item.Value );

                if( item.TokenType == TokenType.ObjectRef )
                {
                    var indexOfFirstDot = item.Value.IndexOf( '.' );
                    if( indexOfFirstDot < 0 )
                        throw new Exception("Invalid object reference.");

                    var objRef = item.Value.Substring( 0, indexOfFirstDot );
                    var path = item.Value.Substring( indexOfFirstDot + 1 );

                    var o = lookup.GetObjectById( objRef );
                    var val = GetDeepPropertyValue( o, path );

                    sb.Append( val );
                }
            }


            return sb.ToString();
        }


        private object GetDeepPropertyValue( object instance, string path )
        {
            var pp = path.Split( '.' );
            Type t = instance.GetType();
            foreach( var prop in pp )
            {
                var pi = t.GetProperty( prop );
                if( pi != null )
                {
                    instance = pi.GetValue( instance, null );
                    t = pi.PropertyType;
                }

                else throw new ArgumentException( "Properties path is not correct" );
            }

            return instance;
        }

    }


    public class Token
    {
        TokenType _tokenType;
        string _value;

        public Token( TokenType tokenType, string value )
        {
            _tokenType = tokenType;
            _value = value;
        }

        public TokenType TokenType => _tokenType;
        public string Value => _value;
    }

    public enum TokenType
    {
        String,
        ObjectRef
    }
}
