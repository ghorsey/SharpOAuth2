using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOAuth2.Provider.AuthorizationEndpoint;
using SharpOAuthProvider.Domain.Repository;
using SharpOAuth2.Provider.Services;

namespace SharpOAuthProvider.Domain.Service
{
    public class ClientService : IClientService
    {
        readonly IClientRepository ClientRepo;

        public ClientService(IClientRepository repo)
        {
            ClientRepo = repo;
        }

        #region IClientService Members

        public bool AuthenticateClient(IAuthorizationContext context)
        {
            Client actual = ClientRepo.LoadClient(context.Client.ClientId);
            if (actual == null) return false;

            return actual.ClientSecret == context.Client.ClientSecret;
        }

        public bool ValidateRedirectUri(IAuthorizationContext context)
        {
            Client cl = ClientRepo.LoadClient(context.Client.ClientId);
            return context.RedirectUri.Equals(cl.RedirectUri);
        }

        public bool IsClient(IAuthorizationContext context)
        {
            return ClientRepo.LoadClient(context.Client.ClientId) != null;
        }


        public bool AuthenticateClient(SharpOAuth2.Provider.IOAuthContext context)
        {
            throw new NotImplementedException();
        }

        public bool ValidateRedirectUri(SharpOAuth2.Provider.IOAuthContext context)
        {
            throw new NotImplementedException();
        }

        public bool IsClient(SharpOAuth2.Provider.IOAuthContext context)
        {
            throw new NotImplementedException();
        }

        public SharpOAuth2.ClientBase FindClient(string clientId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
