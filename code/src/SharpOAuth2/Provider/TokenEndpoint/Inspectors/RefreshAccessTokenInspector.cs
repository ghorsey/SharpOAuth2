using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2.Provider.TokenEndpoint.Inspectors
{
    public class RefreshAccessTokenInspector : IContextInspector<ITokenContext>
    {
        #region IContextInspector<ITokenContext> Members

        public void Inspect(ITokenContext context)
        {
            if (context.GrantType != Parameters.GrantTypeValues.RefreshToken) return;

            if (string.IsNullOrWhiteSpace(context.RefreshToken))
                throw Errors.InvalidRequestException(context, Parameters.RefreshToken);
        }

        #endregion
    }
}
