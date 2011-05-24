using System.Security.Principal;
using SharpOAuth2.Framework;

namespace SharpOAuth2.Provider.ResourceAuthorizationHttpModule
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