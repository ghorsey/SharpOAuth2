using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2.Provider
{
    [Serializable]
    public class OAuthErrorResponseException<T> : Exception where T : IOAuthContext
    {
        public T Context{ get; private set; }
        public string Error { get; private set; }
        public Uri ErrorUri { get; private set; }

        public OAuthErrorResponseException(T context, string error)
            : this()
        {
            Context = context;
            Error = error;
        }

        public OAuthErrorResponseException(T context, string error, string description = "", Uri errorUri = null)
            : this( description )
        {
            ErrorUri = errorUri;
            Error = error;
            Context = context;
        }
        public OAuthErrorResponseException(T context, string error, string description, Exception inner, Uri errorUri = null)
            : this(description, inner)
        {
            ErrorUri = errorUri;
            Error = error;
            Context = context;
        }

        private OAuthErrorResponseException() { }
        private OAuthErrorResponseException(string message) : base(message) { }
        private OAuthErrorResponseException(string message, Exception inner) : base(message, inner) { }
        protected OAuthErrorResponseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
