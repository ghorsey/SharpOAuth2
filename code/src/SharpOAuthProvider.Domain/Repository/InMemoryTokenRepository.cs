using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public AuthorizationGrant LoadAuthroizationGrant(string code)
        {
            if (!_grants.ContainsKey(code)) return null;

            return _grants[code];
        }

        public SharpOAuth2.AuthorizationGrantBase FindAuthorizationGrant(string authorizationCode)
        {
            if (!_grants.ContainsKey(authorizationCode)) return null;
            return _grants[authorizationCode];
        }


        public void AddAccessToken(AccessToken token)
        {
            _tokens[token.Token] = token;
        }

        #endregion
    }
}
