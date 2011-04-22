using System.Linq;
using SharpOAuth2.Framework;
using SharpOAuth2.Provider.Domain;
using SharpOAuth2.Provider.Framework;
using SharpOAuth2.Provider.Services;
using SharpOAuthProvider.Domain.Repository;

namespace SharpOAuthProvider.Domain.Service
{
    public class ClientService : IClientService
    {
        readonly IClientRepository ClientRepo;
        readonly ITokenRepository TokenRepo;

        public ClientService(IClientRepository repo, ITokenRepository tokenRepo)
        {
            ClientRepo = repo;
            TokenRepo = tokenRepo;
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


        public ClientBase FindClient(string clientId)
        {
            return ClientRepo.FindClient(clientId);
        }

        public bool IsAccessGranted(IClient client, string[] scope, string resourceOwnerId)
        {
            AuthorizationGrant grant = TokenRepo.FindAuthorizationGrant(client.ClientId, resourceOwnerId);
            if (grant == null) return false;
            if (!grant.IsApproved) return false;

            return  scope.Where(x => !grant.Scope.Contains(x.ToLower())).Count() == 0;
        }

        #endregion
    }
}
