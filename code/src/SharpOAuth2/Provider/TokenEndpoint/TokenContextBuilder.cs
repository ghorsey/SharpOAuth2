using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Web;
using SharpOAuth2.Globalization;

namespace SharpOAuth2.Provider.TokenEndpoint
{
    public class TokenContextBuilder : IContextBuilder<ITokenContext>
    {
       
        private ITokenContext CreateContext(NameValueCollection parameters)
        {
            return new TokenContext()
            {
                Client = new ClientBase
                {
                    ClientId = parameters[Parameters.ClientId],
                    ClientSecret = parameters[Parameters.ClientSecret]
                },
                AuthorizationCode = parameters[Parameters.AuthroizationCode],
                GrantType = parameters[Parameters.GrantType],
                RedirectUri = ContextBuilderHelpers.CreateRedirectUri(parameters[Parameters.RedirectUri]),
                Username = parameters[Parameters.ResourceOwnerUsername],
                Password = parameters[Parameters.ResourceOwnerPassword],
                RefreshToken = parameters[Parameters.RefreshToken],
                Scope = ContextBuilderHelpers.CreateScope(parameters[Parameters.Scope])
            };
            
        }
        #region IContextBuilder<ITokenContext> Members

        public ITokenContext FromUri(string url)
        {
            throw new NotSupportedException(TokenEndpointResources.InvalidHttpMethodTokenRequest);
        }

        public ITokenContext FromUri(Uri uri)
        {
            throw new NotSupportedException(TokenEndpointResources.InvalidHttpMethodTokenRequest);
        }

        public ITokenContext FromHttpRequest(System.Web.HttpRequestBase request)
        {
            if (request.HttpMethod.ToUpperInvariant() != "POST")
                throw new OAuthFatalException(TokenEndpointResources.InvalidHttpMethodTokenRequest);

            return CreateContext(request.Form);

        }

        #endregion
    }
}
