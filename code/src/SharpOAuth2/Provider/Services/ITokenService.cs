using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOAuth2.Provider.AuthorizationEndpoint;
using SharpOAuth2.Provider.TokenEndpoint;

namespace SharpOAuth2.Provider.Services
{
    public interface ITokenService
    {
        AuthorizationGrantBase MakeAuthorizationGrant(IAuthorizationContext context);
        void ApproveAuthorizationGrant(AuthorizationGrantBase authorizationGrant, bool isApproved);

        AuthorizationGrantBase FindAuthorizationGrant(string authorizationCode);

        void ConsumeGrant(AuthorizationGrantBase grant);
        AccessTokenBase MakeAccessToken(AuthorizationGrantBase grant);

        bool ValidateRedirectUri(IOAuthContext context, AuthorizationGrantBase grant);

        AccessTokenBase FindToken(string token);

        IToken MakeAccessToken(string resourceOwnerUsername);

        AccessTokenBase MakeAccessToken(ClientBase client);
    }
}
