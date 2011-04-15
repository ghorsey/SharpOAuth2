using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOAuthProvider.Domain.Repository;
using SharpOAuth2.Provider.AuthorizationEndpoint;
using SharpOAuth2;
using SharpOAuth2.Provider.Services;

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
            Client client = ClientRepo.FindClient(context.Client.ClientId);
            grant.Client = client;
            grant.ExpiresIn = 120; // 2 minutes
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


        public AuthorizationGrantBase FindAuthorizationGrant(string authorizationCode)
        {
            return TokenRepo.FindAuthorizationGrant(authorizationCode);
        }


        public void ConsumeGrant(AuthorizationGrantBase grant)
        {
            AuthorizationGrant gr = (AuthorizationGrant)grant;
            gr.IsUsed = true;
            TokenRepo.AddAuthorizationGrant(gr);
        }

        public AccessTokenBase MakeAccessToken(AuthorizationGrantBase grant)
        {
            AccessToken token = new AccessToken
            {
                ExpiresIn = 120,
                Token = Guid.NewGuid().ToString(),
                TokenType = "bearer",
                Grant = (AuthorizationGrant)grant
            };

            TokenRepo.AddAccessToken(token);
            return token;

        }

        public bool ValidateRedirectUri(SharpOAuth2.Provider.IOAuthContext context, AuthorizationGrantBase grant)
        {
            return grant.RedirectUri.Equals(context.RedirectUri);
        }

        #endregion
    }
}
