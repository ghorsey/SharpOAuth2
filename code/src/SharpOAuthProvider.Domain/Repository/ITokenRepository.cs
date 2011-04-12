using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuthProvider.Domain.Repository
{
    public interface ITokenRepository
    {

        void AddAuthorizationGrant(AuthorizationGrant grant);
        AuthorizationGrant LoadAuthroizationGrant(string code);
    }
}
