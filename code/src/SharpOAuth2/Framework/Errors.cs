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
using System.Globalization;
using SharpOAuth2.Framework;
using SharpOAuth2.Provider.Globalization;
using SharpOAuth2.Provider.ResourceEndpoint;
using SharpOAuth2.Provider.TokenEndpoint;

namespace SharpOAuth2.Provider.Framework
{
    internal static class Errors
    {
        internal static OAuthErrorResponseException<T> InvalidRequestException<T>(T context, string parameter, Uri uri = null) where T : class
        {
            if (string.IsNullOrWhiteSpace(parameter))
                throw new ArgumentException("parameter");
            if (context == null)
                throw new ArgumentNullException("context");

            return new OAuthErrorResponseException<T>(context,
                Parameters.ErrorParameters.ErrorValues.InvalidRequest,
                string.Format(CultureInfo.CurrentUICulture, ErrorResponseResources.InvalidRequest, parameter),
                errorUri: uri);

        }
        internal static OAuthErrorResponseException<T> UnsupportedResponseType<T>(T context, string responseType, Uri uri = null) where T : class
        {
            if (string.IsNullOrWhiteSpace(responseType))
                throw new ArgumentException("responseType");
            if (context == null)
                throw new ArgumentNullException("context");

            return new OAuthErrorResponseException<T>(context,
                Parameters.ErrorParameters.ErrorValues.UnsupportedResponseType, 
                string.Format(CultureInfo.CurrentUICulture, ErrorResponseResources.InvalidResponseType, responseType),
                 errorUri: uri);
        }

        internal static OAuthErrorResponseException<T> AccessDenied<T>(T context) where T : class
        {
            return new OAuthErrorResponseException<T>(context,
                        Parameters.ErrorParameters.ErrorValues.AccessDenied,
                        description: AuthorizationEndpointResources.ResourceOwnerDenied);
        }

        internal static OAuthErrorResponseException<T> UnauthorizedClient<T>(T context, IClient client) where T : class
        {
            return new OAuthErrorResponseException<T>(context,
                Parameters.ErrorParameters.ErrorValues.UnauthorizedClient,
                httpStatusCode: 401,
                description: string.Format(CultureInfo.CurrentUICulture, AuthorizationEndpointResources.InvalidClient, client.ClientId));
        }

        internal static OAuthErrorResponseException<IOAuthContext> InvalidClient(IOAuthContext context)
        {
            
            return new OAuthErrorResponseException<IOAuthContext>(context,
                Parameters.ErrorParameters.ErrorValues.InvalidClient,
                httpStatusCode: 401,
                description: string.Format(CultureInfo.CurrentUICulture,
                TokenEndpointResources.InvalidClientCredentials, context.Client.ClientId));
        }
        internal static OAuthErrorResponseException<ITokenContext> InvalidGrant(ITokenContext context) 
        {
            return new OAuthErrorResponseException<ITokenContext>(context,
                Parameters.ErrorParameters.ErrorValues.InvalidGrant,
                description: string.Format( CultureInfo.CurrentUICulture,
                TokenEndpointResources.InvalidGrant,
                context.AuthorizationCode));
        }

        internal static OAuthErrorResponseException<ITokenContext> UnsupportedGrantType(ITokenContext context)
        {
            return new OAuthErrorResponseException<ITokenContext>(context,
                Parameters.ErrorParameters.ErrorValues.UnsupportedGrantType,
                description: string.Format(CultureInfo.CurrentUICulture,
                TokenEndpointResources.UnsupportedGrantType,
                context.GrantType));
        }

        internal static Exception InvalidToken(IResourceContext context)
        {
            return new OAuthErrorResponseException<IResourceContext>(context,
                Parameters.ErrorParameters.ErrorValues.InvalidToken,
                httpStatusCode: 401);
        }

        internal static OAuthErrorResponseException<IResourceContext> InsufficientScope(IResourceContext context, string requiredScope)
        {
            return new OAuthErrorResponseException<IResourceContext>(context,
                Parameters.ErrorParameters.ErrorValues.InsufficientScope,
                string.Format(CultureInfo.CurrentUICulture,
                ResourceEndpointResources.InsufficientScope, requiredScope),
                httpStatusCode: 403);
 
        }
    }
}
