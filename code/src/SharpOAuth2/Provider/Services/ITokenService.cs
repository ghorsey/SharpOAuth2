using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOAuth2.Provider.AuthorizationEndpoint;

namespace SharpOAuth2.Provider.Services
{
    public interface ITokenService
    {
        AuthorizationGrantBase MakeAuthorizationGrant(IAuthorizationContext context);
        void ApproveAuthorizationGrant(AuthorizationGrantBase authorizationGrant, bool isApproved);
    }
}
