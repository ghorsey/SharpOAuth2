using System.Collections.Generic;
using System.Linq;
using SharpOAuth2.Provider.Domain;

namespace SharpOAuthProvider.Domain.Repository
{
    public class InMemoryTokenRepository : ITokenRepository
    {
        static readonly IDictionary<string, AuthorizationGrant> GrantRepo = new Dictionary<string, AuthorizationGrant>();
        static readonly IDictionary<string, AccessToken> TokensRepo = new Dictionary<string, AccessToken>();
        static readonly IDictionary<string, RefreshTokenBase> RefreshTokenRepo = new Dictionary<string, RefreshTokenBase>();
        static InMemoryTokenRepository()
        {
            RefreshTokenRepo["refresh"] = new RefreshTokenBase
            {
                ClientId = "12345",
                Scope = new string[] { "create", "view", "delete"},
                Token = "refresh"
            };
        }

        #region ITokenRepository Members

        public void AddAuthorizationGrant(AuthorizationGrant grant)
        {
            GrantRepo[grant.Code] = grant;
        }

        public AuthorizationGrant FindAuthorizationGrant(string authorizationCode)
        {
            if (!GrantRepo.ContainsKey(authorizationCode)) return null;
            return GrantRepo[authorizationCode];
        }


        public void AddAccessToken(AccessToken token)
        {
            TokensRepo[token.Token] = token;
        }

        public AccessTokenBase FindToken(string token)
        {
            if (!TokensRepo.ContainsKey(token))
                return null;

            return TokensRepo[token];
        }

        public AuthorizationGrant FindAuthorizationGrant(string clientId, string resourceOwnerId)
        {
            return (from x in GrantRepo
                    where x.Value.Client.ClientId.ToUpperInvariant() == clientId.ToUpperInvariant() &&
                    x.Value.ResourceOwnerId.ToUpperInvariant() == resourceOwnerId.ToUpperInvariant()
                    orderby x.Value.IssuedOn descending
                    select x.Value).FirstOrDefault();
        }


        public RefreshTokenBase FindRefreshToken(string refreshToken)
        {
            if (!RefreshTokenRepo.ContainsKey(refreshToken))
                return null;

            return RefreshTokenRepo[refreshToken];
        }

        #endregion
    }
}
