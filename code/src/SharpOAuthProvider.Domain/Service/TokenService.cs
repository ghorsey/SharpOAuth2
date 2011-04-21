using System;
using SharpOAuth2.Provider.AuthorizationEndpoint;
using SharpOAuth2.Provider.Domain;
using SharpOAuth2.Provider.Framework;
using SharpOAuth2.Provider.Services;
using SharpOAuthProvider.Domain.Repository;

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

        public AuthorizationGrantBase IssueAuthorizationGrant(IAuthorizationContext context)
        {
            AuthorizationGrant grant = new AuthorizationGrant();
            Client client = ClientRepo.FindClient(context.Client.ClientId);
            grant.Client = client;
            grant.ExpiresIn = 120; // 2 minutes
            grant.Scope = context.Scope;
            grant.ResourceOwnerId = context.ResourceOwnerUsername;
            grant.Token = Guid.NewGuid().ToString();
            return grant;
        }

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


        public void ConsumeGrant(AuthorizationGrantBase grant)
        {
            AuthorizationGrant gr = (AuthorizationGrant)grant;
            gr.IsUsed = true;
            TokenRepo.AddAuthorizationGrant(gr);
        }

        public IToken IssueAccessToken(AuthorizationGrantBase grant)
        {
            AccessToken token = new AccessToken
            {
                ExpiresIn = 120,
                Token = Guid.NewGuid().ToString(),
                Grant = (AuthorizationGrant)grant
            };
            token.Scope = grant.Scope;

            TokenRepo.AddAccessToken(token);
            return token;

        }

        public bool ValidateRedirectUri(IOAuthContext context, AuthorizationGrantBase grant)
        {
            return grant.RedirectUri.Equals(context.RedirectUri);
        }


        public AccessTokenBase FindToken(string token)
        {
            return TokenRepo.FindToken(token);
        }

        public IToken IssueAccessToken(string resourceOwnerUsername)
        {
            AccessToken token = new AccessToken
            {
                ExpiresIn = 120,
                Token = Guid.NewGuid().ToString(),
                RefreshToken = Guid.NewGuid().ToString(),
                Scope = new string[] { "create", "delete", "view" },
                ResourceOwnerUsername = resourceOwnerUsername
            };
            TokenRepo.AddAccessToken(token);
            return token;
        }

        public IToken IssueAccessToken(ClientBase client)
        {
            AccessToken token = new AccessToken
            {
                ExpiresIn = 120,
                Token = Guid.NewGuid().ToString(),
                Scope = new string[] { "create-member" },
                Client = (Client)client
            };

            TokenRepo.AddAccessToken(token);

            return token;
        }

        public IToken IssueAccessToken(RefreshTokenBase refreshToken)
        {
            AccessToken token = new AccessToken
            {
                ExpiresIn = 120,
                Token = Guid.NewGuid().ToString(),
                RefreshToken = refreshToken.Token,
                ResourceOwnerUsername = refreshToken.ResourceOwnerUsername,
                Scope = refreshToken.Scope,
                TokenType = "bearer"
            };
            TokenRepo.AddAccessToken(token);
            return token;
        }

        public RefreshTokenBase FindRefreshToken(string refreshToken)
        {
            return TokenRepo.FindRefreshToken(refreshToken);
        }

        #endregion
    }
}
