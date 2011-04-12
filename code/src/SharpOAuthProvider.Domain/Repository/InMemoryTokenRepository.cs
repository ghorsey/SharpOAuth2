using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuthProvider.Domain.Repository
{
    public class InMemoryTokenRepository : ITokenRepository
    {
        static readonly IDictionary<string, AuthorizationGrant> _grants = new Dictionary<string, AuthorizationGrant>();

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

        #endregion
    }
}
