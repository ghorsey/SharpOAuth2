using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2.Provider.TokenEndpoint
{
    public interface ITokenContext : IOAuthContext
    {
        string GrantType { get; set; }
        string AuthorizationCode { get; set; }
        string RefreshToken { get; set; }
        string ResourceOwnerUsername { get; set; }
        string ResourceOwnerPassword { get; set; }
    }
}
