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
        string Username { get; set; }
        string Password { get; set; }
    }
}
