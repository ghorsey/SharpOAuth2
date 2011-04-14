using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOAuth2.Provider.Services;

namespace SharpOAuth2.Provider.TokenEndpoint
{
    public class TokenProvider : ITokenProvider
    {
        readonly IServiceFactory ServiceFactory;

        #region ITokenProvider Members

        public void GrantAuthorizationToken(ITokenContext context)
        {
            
        }

        #endregion
    }
}
