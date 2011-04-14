using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2.Provider.AuthorizationEndpoint.Inspectors
{
    public class SpecificationInspector : IContextInspector<IAuthorizationContext>
    {
        #region IAuthorizationContextInspector Members

        public void Inspect(IAuthorizationContext context)
        {
            if (string.IsNullOrWhiteSpace(context.ResponseType))
                throw Errors.InvalidRequestException(context, Parameters.ResponseType);

            if (context.Client == null || string.IsNullOrWhiteSpace(context.Client.ClientId))
                throw Errors.InvalidRequestException(context, Parameters.ClientId);

            if (context.RedirectUri == null)
                throw Errors.InvalidRequestException(context, Parameters.RedirectUri);

            if (context.ResponseType != Parameters.ResponseTypeValues.AccessToken && context.ResponseType != Parameters.ResponseTypeValues.AuthorizationCode)
                throw Errors.UnsupportedResponseType(context, context.ResponseType);

        }

        #endregion
    }
}
