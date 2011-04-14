using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using SharpOAuth2.Globalization;

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

        public static OAuthErrorResponseException<T> AccessDenied<T>(T context) where T: class
        {
            return new OAuthErrorResponseException<T>(context,
                        Parameters.ErrorParameters.ErrorValues.AccessDenied,
                        description: AuthorizationEndpointResources.ResourceOwnerDenied);
        }

        public static OAuthErrorResponseException<T> UnauthorizedClient<T>(T context, IClient client) where T: class
        {
            return new OAuthErrorResponseException<T>(context,
                Parameters.ErrorParameters.ErrorValues.UnauthorizedClient,
                description: string.Format(CultureInfo.CurrentUICulture, AuthorizationEndpointResources.InvalidClient, client.ClientId));
        }
    }
}
