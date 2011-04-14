using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2.Provider.AuthorizationEndpoint
{
    public interface IAuthorizationResponseBuilder
    {
        Uri CreateResponse(IAuthorizationContext context);
    }
}
