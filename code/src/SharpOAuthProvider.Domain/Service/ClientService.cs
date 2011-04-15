using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOAuth2.Provider.AuthorizationEndpoint;
using SharpOAuthProvider.Domain.Repository;
using SharpOAuth2.Provider.Services;
using SharpOAuth2.Provider;

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

        public bool AuthenticateClient(IOAuthContext context)
        {
            Client actual = ClientRepo.FindClient(context.Client.ClientId);
            if (actual == null) return false;

            return actual.ClientSecret == context.Client.ClientSecret;
        }

        public bool ValidateRedirectUri(IOAuthContext context)
        {
            Client cl = ClientRepo.FindClient(context.Client.ClientId);
            return context.RedirectUri.Equals(cl.RedirectUri);
        }

        public bool IsClient(IOAuthContext context)
        {
            return ClientRepo.FindClient(context.Client.ClientId) != null;
        }


        public SharpOAuth2.ClientBase FindClient(string clientId)
        {
            return ClientRepo.FindClient(clientId);
        }

        #endregion
    }
}
