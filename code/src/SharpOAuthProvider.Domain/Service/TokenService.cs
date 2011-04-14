using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOAuth2.Provider.AuthorizationEndpoint.Services;
using SharpOAuthProvider.Domain.Repository;
using SharpOAuth2.Provider.AuthorizationEndpoint;
using SharpOAuth2;

namespace SharpOAuthProvider.Domain.Service
{
    public class TokenService : ITokenService
    {
        readonly IClientRepository ClientRepo;
        readonly ITokenRepository TokenRepo;

        public TokenService(IClientRepository clientRepo, ITokenRepository tokenRepo)
        {
            ClientRepo = clientRepo;
            TokenRepo = tokenRepo;
        }

        #region ITokenService Members

        public AuthorizationGrantBase MakeAuthorizationGrant(IAuthorizationContext context)
        {
            AuthorizationGrant grant = new AuthorizationGrant();
            Client client = ClientRepo.LoadClient(context.Client.ClientId);
            grant.Client = client;
            grant.Expires = 120; // 2 minutes
            grant.Scope = context.Scope;
            grant.ResourceOwnerId = context.ResourceOwnerId;
            grant.Token = Guid.NewGuid().ToString();
            return grant;
        }

        public void ApproveAuthorizationGrant(SharpOAuth2.AuthorizationGrantBase authorizationGrant, bool isApproved)
        {
            AuthorizationGrant grant = (AuthorizationGrant)authorizationGrant;
            grant.IsUsed = false;
            grant.IsApproved = isApproved;

            TokenRepo.AddAuthorizationGrant(grant);
        }

        #endregion
    }
}
