using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using SharpOAuth2.Framework;

namespace RESTSample
{
    public class TokenPrincipal : GenericPrincipal
    {
        public IToken Token { get; private set; }
        public TokenPrincipal(IIdentity identity, string[] roles, IToken token)
            : base(identity, roles)
        {
            Token = token;
        }
    }
}