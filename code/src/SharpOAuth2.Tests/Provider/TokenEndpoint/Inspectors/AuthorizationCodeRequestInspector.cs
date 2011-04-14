using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOAuth2.Provider.TokenEndpoint;
using SharpOAuth2.Provider;

namespace SharpOAuth2.Tests.Provider.TokenEndpoint.Inspectors
{
    public class AuthorizationCodeRequestInspector : IContextInspector<ITokenContext>
    {
        #region IContextInspector<ITokenContext> Members

        public void Inspect(ITokenContext context)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
