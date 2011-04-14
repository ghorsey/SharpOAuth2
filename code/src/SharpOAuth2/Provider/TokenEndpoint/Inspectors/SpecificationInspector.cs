using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2.Provider.TokenEndpoint.Inspectors
{
    public class SpecificationInspector : IContextInspector<ITokenContext>
    {
        #region IContextInspector<ITokenContext> Members

        public void Inspect(ITokenContext context)
        {
            string[] allowedGrantTypes = new string[]
            {
                Parameters.GrantTypeValues.AuthorizationCode,
                Parameters.GrantTypeValues.ClientCredentials,
                Parameters.GrantTypeValues.Password,
                Parameters.GrantTypeValues.RefreshToken
            };

            if (!allowedGrantTypes.Contains(context.GrantType))
                throw Errors.InvalidRequestException(context, Parameters.GrantType);

        }

        #endregion
    }
}
