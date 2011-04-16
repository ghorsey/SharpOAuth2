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
        AuthorizationGrant LoadAuthroizationGrant(string code);

        AuthorizationGrantBase FindAuthorizationGrant(string authorizationCode);

        AccessTokenBase FindToken(string token);
    }
}
