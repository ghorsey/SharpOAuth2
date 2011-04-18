using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOAuth2;

namespace SharpOAuthProvider.Domain.Repository
{
    public interface ITokenRepository
    {
        void AddAccessToken(AccessToken token);
        void AddAuthorizationGrant(AuthorizationGrant grant);

        AuthorizationGrant FindAuthorizationGrant(string authorizationCode);
        AuthorizationGrant FindAuthorizationGrant(string clientId, string resourceOwnerId);
        AccessTokenBase FindToken(string token);

    }
}
