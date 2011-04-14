using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2.Provider.TokenEndpoint.Inspectors
{
    public class ResourceOwnerPasswordCredentialInspector : IContextInspector<ITokenContext>
    {
        #region IContextInspector<ITokenContext> Members

        public void Inspect(ITokenContext context)
        {
            if (context.GrantType != Parameters.GrantTypeValues.Password) return;

            if (string.IsNullOrWhiteSpace(context.ResourceOwnerUsername))
                throw Errors.InvalidRequestException(context, Parameters.ResourceOwnerUsername);

            if (string.IsNullOrWhiteSpace(context.ResourceOwnerPassword))
                throw Errors.InvalidRequestException(context, Parameters.ResourceOwnerPassword);
        }

        #endregion
    }
}
