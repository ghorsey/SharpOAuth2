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

        bool AuthorizationGrantIsValid(AuthorizationGrantBase grant);

        void SetAccessToken(ITokenContext context);
    }
}
