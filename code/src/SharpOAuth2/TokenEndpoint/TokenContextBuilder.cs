#region License
/* The MIT License
 * 
 * Copyright (c) 2011 Geoff Horsey
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
#endregion

using System;
using System.Collections.Specialized;
using SharpOAuth2.Provider.Domain;
using SharpOAuth2.Provider.Framework;
using SharpOAuth2.Provider.Globalization;
using SharpOAuth2.Provider.Utility;

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
                ResourceOwnerUsername = parameters[Parameters.ResourceOwnerUsername],
                ResourceOwnerPassword = parameters[Parameters.ResourceOwnerPassword],
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
