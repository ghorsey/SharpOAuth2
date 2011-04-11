using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2.Provider.Authorization.Inspectors
{
    public class SpecificationInspector : IAuthorizationContextInspector
    {
        #region IAuthorizationContextInspector Members

        public void Insepct(IAuthorizationContext context)
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
