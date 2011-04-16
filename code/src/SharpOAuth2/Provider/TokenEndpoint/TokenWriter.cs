using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2.Provider.TokenEndpoint
{
    public abstract class TokenWriter<T>  where T : class
    {
        protected readonly T Destination;
        public TokenWriter(T destination)
        {
            Destination = destination;
        }
        public abstract void WriteResponse(TokenResponse tokenResponse);
    }
}
