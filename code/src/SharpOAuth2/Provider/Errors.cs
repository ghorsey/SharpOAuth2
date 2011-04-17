using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using SharpOAuth2.Globalization;
using SharpOAuth2.Provider.TokenEndpoint;
using SharpOAuth2.Provider.ResourceEndpoint;

namespace SharpOAuth2.Provider
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
                description: string.Format(CultureInfo.CurrentUICulture, AuthorizationEndpointResources.InvalidClient, client.ClientId));
        }

        internal static OAuthErrorResponseException<IOAuthContext> InvalidClient(IOAuthContext context)
        {
            
            return new OAuthErrorResponseException<IOAuthContext>(context,
                Parameters.ErrorParameters.ErrorValues.InvalidClient,
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
