using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOAuth2;

namespace SharpOAuthProvider.Domain
{
    public class AccessToken : AccessTokenBase
    {
        public AuthorizationGrant Grant { get; set; }

        public AccessToken()
        {
            TokenType = "bearer";
        }
    }
}
