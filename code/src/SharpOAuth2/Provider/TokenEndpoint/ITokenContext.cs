using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2.Provider.TokenEndpoint
{
    public interface ITokenContext
    {
        string GrantType { get; set; }
        string Code { get; set; }
        Uri RedirectUri { get; set; }
        IClient Client { get; set; }
    }
}
