using System;
using SharpOAuth2.Provider.Domain;
using SharpOAuth2.Provider.Services;
using SharpOAuthProvider.Domain.Repository;
using SharpOAuth2.Provider.AuthorizationEndpoint;

namespace SharpOAuthProvider.Domain.Service
{
    public class AuthorizationGrantService : IAuthorizationGrantService
    {
        readonly IClientRepository ClientRepo;
        readonly ITokenRepository TokenRepo;
        public AuthorizationGrantService(ITokenRepository tokenRepo, IClientRepository clientRepo)
        {
            TokenRepo = tokenRepo;
            ClientRepo = clientRepo;
        }
        #region IAuthorizationGrantService Members

        public void ApproveAuthorizationGrant(AuthorizationGrantBase authorizationGrant, bool isApproved)
        {
            AuthorizationGrant grant = (AuthorizationGrant)authorizationGrant;

            grant.IsUsed = false;
            grant.IsApproved = isApproved;

            TokenRepo.AddAuthorizationGrant(grant);
        }

        public AuthorizationGrantBase FindAuthorizationGrant(string authorizationCode)
        {
            return TokenRepo.FindAuthorizationGrant(authorizationCode);
        }


        public AuthorizationGrantBase IssueAuthorizationGrant(IAuthorizationContext context)
        {
            AuthorizationGrant grant = new AuthorizationGrant();
            Client client = ClientRepo.FindClient(context.Client.ClientId);
            grant.Client = client;
            grant.ResourceOwnerId = context.ResourceOwnerUsername;
            grant.IsApproved = false;
            grant.ExpiresIn = 120; // 2 minutes
            grant.Code = Guid.NewGuid().ToString();
            return grant;
        }

        public void ConsumeGrant(AuthorizationGrantBase grant)
        {
            AuthorizationGrant gr = (AuthorizationGrant)grant;
            gr.IsUsed = true;
            TokenRepo.AddAuthorizationGrant(gr);
        }


        public bool ValidateGrant(SharpOAuth2.Provider.TokenEndpoint.ITokenContext context, AuthorizationGrantBase grant)
        { 
            return true; // should validate expiration, occurance
        }

        #endregion
    }
}
