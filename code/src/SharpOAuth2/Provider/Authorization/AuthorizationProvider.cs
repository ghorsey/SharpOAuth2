using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;
using SharpOAuth2.Provider.Authorization.Inspectors;
using SharpOAuth2.Provider.Authorization.Services;
using SharpOAuth2.Globalization;
using System.Globalization;

namespace SharpOAuth2.Provider.Authorization
{
    public class AuthorizationProvider : IAuthorizationProvider
    {
        readonly IAuthorizationServiceFactory ServiceFactory;

        public AuthorizationProvider(IAuthorizationServiceFactory serviceFactory)
        {
            ServiceFactory = serviceFactory;
        }
        private void InspectRequest(IAuthorizationContext context)
        {
            IEnumerable<IAuthorizationContextInspector> inspectors = ServiceLocator.Current.GetAllInstances<IAuthorizationContextInspector>();

            try
            {
                foreach (IAuthorizationContextInspector inspector in inspectors)
                    inspector.Insepct(context);
            }
            catch (OAuthErrorResponseException<IAuthorizationContext> exception)
            {
                context.Error = new ErrorResponse
                {
                    Error = exception.Error,
                    ErrorDescription = exception.Message,
                    ErrorUri = exception.ErrorUri
                };
            }
        }

        private void AssertNoAuthorizationToken(IAuthorizationContext context)
        {
            if (context.Authorization != null)
                throw new OAuthFatalException(AuthorizationResources.AuthorizationContextContainsToken);
        }

        private void AssertIsClient(IAuthorizationContext context)
        {
            if (!ServiceFactory.ClientService.IsClient(context))
                throw new OAuthFatalException(string.Format(CultureInfo.CurrentUICulture,
                    AuthorizationResources.InvalidClient, context.Client.ClientId));
        }

        private void AssertRedirectUriIsValid(IAuthorizationContext context)
        {
            if (!ServiceFactory.ClientService.ValidateRedirectUri(context))
                throw new OAuthFatalException(string.Format(CultureInfo.CurrentUICulture,
                    AuthorizationResources.InvalidRedirectUri, context.RedirectUri.ToString()));
        }
        #region IAuthorizationProvider Members

        public void CreateAuthorizationGrant(IAuthorizationContext context, bool isApproved)
        {
            InspectRequest(context);
            AssertNoAuthorizationToken(context);
            AssertIsClient(context);
            AssertRedirectUriIsValid(context);

            if (context.Error != null)
                return; // we have an error and we're done

            if (context.ResponseType == Parameters.ResponseTypeValues.AccessToken)
                throw new NotSupportedException();
            else
            {
                AuthorizationGrantBase grant = ServiceFactory.TokenService.MakeAuthorizationGrant(context);
                ServiceFactory.TokenService.ApproveAuthorizationGrant(grant, isApproved);
                context.IsApproved = isApproved;
                context.Authorization = grant;
            }

            if (!isApproved)
            {
                context.Error = new ErrorResponse
                {
                    Error = Parameters.ErrorParameters.ErrorValues.AccessDenied,
                    ErrorDescription = AuthorizationResources.ResourceOwnerDenied
                };
            }
        }

        private void AssertUsernameIsNotBlank(IAuthorizationContext context)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
