using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2.Provider.AuthorizationEndpoint
{
    public class AuthorizationContext : IAuthorizationContext
    {
        #region IAuthorizationContext Members

        public IClient Client{ get; set; }
        public string ResponseType{ get; set; }
        public Uri RedirectUri{ get; set; }
        public string[] Scope{ get; set; }
        public string State{ get; set; }
        public ErrorResponse Error { get; set; }
        public IToken Authorization { get; set; }
        public bool IsApproved { get; set; }
        public string ResourceOwnerId { get; set; }
        #endregion
    }
}
