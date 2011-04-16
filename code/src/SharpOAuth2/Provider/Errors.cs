using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using SharpOAuth2.Globalization;
using SharpOAuth2.Provider.TokenEndpoint;

namespace SharpOAuth2.Provider
{
    public static class Errors
    {
        public static OAuthErrorResponseException<T> InvalidRequestException<T>(T context, string parameter, Uri uri = null) where T : class
        {
            if (string.IsNullOrWhiteSpace(parameter))
                throw new ArgumentException("parameter");
            if (context == null)
                throw new ArgumentNullException("context");

            return new OAuthErrorResponseException<T>(context,
                Parameters.ErrorParameters.ErrorValues.InvalidRequest,
                string.Format(CultureInfo.CurrentUICulture, ErrorResponseResources.InvalidRequest, parameter),
                uri);

        }
        public static OAuthErrorResponseException<T> UnsupportedResponseType<T>(T context, string responseType, Uri uri = null) where T : class
        {
            if (string.IsNullOrWhiteSpace(responseType))
                throw new ArgumentException("responseType");
            if (context == null)
                throw new ArgumentNullException("context");

            return new OAuthErrorResponseException<T>(context,
                Parameters.ErrorParameters.ErrorValues.UnsupportedResponseType, 
                string.Format(CultureInfo.CurrentUICulture, ErrorResponseResources.InvalidResponseType, responseType),
                 uri);
        }

        public static OAuthErrorResponseException<T> AccessDenied<T>(T context) where T : class
        {
            return new OAuthErrorResponseException<T>(context,
                        Parameters.ErrorParameters.ErrorValues.AccessDenied,
                        description: AuthorizationEndpointResources.ResourceOwnerDenied);
        }

        public static OAuthErrorResponseException<T> UnauthorizedClient<T>(T context, IClient client) where T : class
        {
            return new OAuthErrorResponseException<T>(context,
                Parameters.ErrorParameters.ErrorValues.UnauthorizedClient,
                description: string.Format(CultureInfo.CurrentUICulture, AuthorizationEndpointResources.InvalidClient, client.ClientId));
        }

        public static OAuthErrorResponseException<IOAuthContext> InvalidClient(IOAuthContext context)
        {
            
            return new OAuthErrorResponseException<IOAuthContext>(context,
                Parameters.ErrorParameters.ErrorValues.InvalidClient,
                description: string.Format(CultureInfo.CurrentUICulture,
                TokenEndpointResources.InvalidClientCredentials, context.Client.ClientId));
        }
        public static OAuthErrorResponseException<ITokenContext> InvalidGrant(ITokenContext context) 
        {
            return new OAuthErrorResponseException<ITokenContext>(context,
                Parameters.ErrorParameters.ErrorValues.InvalidGrant,
                description: string.Format( CultureInfo.CurrentUICulture,
                TokenEndpointResources.InvalidGrant,
                context.AuthorizationCode));
        }

        public static OAuthErrorResponseException<ITokenContext> UnsupportedGrantType(ITokenContext context)
        {
            return new OAuthErrorResponseException<ITokenContext>(context,
                Parameters.ErrorParameters.ErrorValues.UnsupportedGrantType,
                description: string.Format(CultureInfo.CurrentUICulture,
                TokenEndpointResources.UnsupportedGrantType,
                context.GrantType));
        }
    }
}
