using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2.Provider.TokenEndpoint
{
    public class TokenContext : ITokenContext
    {
        #region ITokenContext Members
        public string GrantType{ get; set; }
        public string AuthorizationCode{ get; set; }
        public Uri RedirectUri{get; set; }
        public IClient Client { get; set; }
        public string Username{ get; set; }
        public string Password{ get; set; }
        public string RefreshToken{ get; set; }
        public string[] Scope{ get; set; }

        #endregion
    }
}
