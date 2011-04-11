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
        readonly IClientService ClientService;
        readonly ITokenService TokenService;

        public AuthorizationProvider(IClientService clientService, ITokenService tokenService)
        {
            ClientService = clientService;
            TokenService = tokenService;
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
        #region IAuthorizationProvider Members

        public void CreateAuthorizationRequest(IAuthorizationContext context)
        {
            InspectRequest(context);
            AssertNoAuthorizationToken(context);

            if (context.ResponseType == Parameters.ResponseTypeValues.AuthorizationCode)
            {
                if (!ClientService.AuthenticateClient(context.Client))
                    throw new OAuthFatalException(string.Format(CultureInfo.CurrentUICulture, 
                        AuthorizationResources.InvalidClient, context.Client.ClientId));
            }
            if (!ClientService.ValidateRedirectUri(context))
                throw new OAuthFatalException(string.Format(CultureInfo.CurrentUICulture,
                    AuthorizationResources.InvalidRedirectUri, context.RedirectUri.ToString()));

            ClientService.ValidateRedirectUri(context);

            context.Authorization = TokenService.MakeRequestToken(context);
        }
        
        public void ApproveAuthorizationRequest(IAuthorizationContext context)
        {
            context.IsApproved = true;
            TokenService.ApproveAuthorizationToken(context.Authorization);
        }

        public void DenyAuthorizationRequest(IAuthorizationContext context)
        {
            context.IsApproved = false;
            TokenService.DenyAuthorizationToken(context.Authorization);
        }

        #endregion
    }
}
