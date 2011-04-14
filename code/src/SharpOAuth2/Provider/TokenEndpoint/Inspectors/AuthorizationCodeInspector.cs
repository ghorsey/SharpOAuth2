using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2.Provider.TokenEndpoint.Inspectors
{
    public class AuthorizationCodeInspector : IContextInspector<ITokenContext>
    {
        #region IContextInspector<ITokenContext> Members

        public void Inspect(ITokenContext context)
        {
            if (context.GrantType != Parameters.GrantTypeValues.AuthorizationCode)
                return; 


            if (string.IsNullOrWhiteSpace(context.AuthorizationCode))
                throw Errors.InvalidRequestException(context, Parameters.AuthroizationCode);

            if (context.RedirectUri == null)
                throw Errors.InvalidRequestException(context, Parameters.RedirectUri);
        }

        #endregion
    }
}
