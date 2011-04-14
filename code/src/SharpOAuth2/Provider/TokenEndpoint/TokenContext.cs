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
        public string Code{ get; set; }
        public Uri RedirectUri{get; set; }
        public IClient Client { get; set; }
        #endregion
    }
}
