using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2.Provider
{
    [Serializable]
    public class OAuthFatalException : Exception
    {
        public OAuthFatalException() { }
        public OAuthFatalException(string message) : base(message) { }
        public OAuthFatalException(string message, Exception inner) : base(message, inner) { }
        protected OAuthFatalException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
