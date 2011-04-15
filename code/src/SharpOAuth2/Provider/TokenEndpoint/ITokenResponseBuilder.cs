using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2.Provider.TokenEndpoint
{
    public interface ITokenResponseBuilder
    {
        TokenResponse CreateResponse(ITokenContext context);
    }
}
