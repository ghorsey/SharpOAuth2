using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2.Provider.Authorization.Services
{
    public interface ITokenService
    {
        IToken MakeRequestToken(IAuthorizationContext context);

        void ApproveAuthorizationToken(IToken authorizationToken);

        void DenyAuthorizationToken(IToken authorizationToken);
    }
}
