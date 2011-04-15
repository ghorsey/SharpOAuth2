using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOAuth2.Globalization;

namespace SharpOAuth2.Provider.TokenEndpoint.Inspectors
{
    public class SpecificationInspector : IContextInspector<ITokenContext>
    {
        #region IContextInspector<ITokenContext> Members

        public void Inspect(ITokenContext context)
        {
            if (string.IsNullOrWhiteSpace(context.GrantType))
                throw Errors.InvalidRequestException(context, Parameters.GrantType);

            if (context.Client == null)
                throw new OAuthFatalException(TokenEndpointResources.MissingClientInContext);

            if (string.IsNullOrWhiteSpace(context.Client.ClientId))
                throw Errors.InvalidRequestException(context, Parameters.ClientId);

            if (string.IsNullOrWhiteSpace(context.Client.ClientSecret))
                throw Errors.InvalidRequestException(context, Parameters.ClientSecret);
        }

        #endregion
    }
}
