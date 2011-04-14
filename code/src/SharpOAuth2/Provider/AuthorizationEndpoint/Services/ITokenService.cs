using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2.Provider.AuthorizationEndpoint.Services
{
    public interface ITokenService
    {
        AuthorizationGrantBase MakeAuthorizationGrant(IAuthorizationContext context);
        void ApproveAuthorizationGrant(AuthorizationGrantBase authorizationGrant, bool isApproved);
    }
}
