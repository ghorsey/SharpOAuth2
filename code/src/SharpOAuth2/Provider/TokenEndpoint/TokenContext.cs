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
        public string ResourceOwnerUsername{ get; set; }
        public string ResourceOwnerPassword{ get; set; }
        public string RefreshToken{ get; set; }
        public string[] Scope{ get; set; }

        #endregion
    }
}
