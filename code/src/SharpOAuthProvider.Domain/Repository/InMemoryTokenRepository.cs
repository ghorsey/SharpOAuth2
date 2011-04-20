using System.Collections.Generic;
using System.Linq;
using SharpOAuth2.Provider.Domain;

namespace SharpOAuthProvider.Domain.Repository
{
    public class InMemoryTokenRepository : ITokenRepository
    {
        static readonly IDictionary<string, AuthorizationGrant> _grants = new Dictionary<string, AuthorizationGrant>();
        static readonly IDictionary<string, AccessToken> _tokens = new Dictionary<string, AccessToken>();

        #region ITokenRepository Members

        public void AddAuthorizationGrant(AuthorizationGrant grant)
        {
            _grants[grant.Token] = grant;
        }

        public AuthorizationGrant FindAuthorizationGrant(string authorizationCode)
        {
            if (!_grants.ContainsKey(authorizationCode)) return null;
            return _grants[authorizationCode];
        }


        public void AddAccessToken(AccessToken token)
        {
            _tokens[token.Token] = token;
        }

        public AccessTokenBase FindToken(string token)
        {
            if (!_tokens.ContainsKey(token))
                return null;

            return _tokens[token];
        }

        public AuthorizationGrant FindAuthorizationGrant(string clientId, string resourceOwnerId)
        {
            return (from x in _grants
                    where x.Value.Client.ClientId.ToUpperInvariant() == clientId.ToUpperInvariant() &&
                    x.Value.ResourceOwnerId.ToUpperInvariant() == resourceOwnerId.ToUpperInvariant()
                    orderby x.Value.IssuedOn descending
                    select x.Value).FirstOrDefault();
        }

        #endregion
    }
}
