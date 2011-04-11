using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2.Provider.Authorization
{
    public interface IAuthorizationProvider
    {
        IToken CreateAuthorizationRequest(IAuthorizationContext context);
        void ApproveAuthorizationRequest(IAuthorizationContext context);
        void DenyAuthorizationRequest(IAuthorizationContext context);

    }
}
