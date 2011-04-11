using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace SharpOAuth2.Provider.Authorization
{
    public interface IAuthorizationContext
    {
        IClient Client { get; set; }
        ErrorResponse Error { get; set; }
        string ResponseType { get; set; }
        Uri RedirectUri { get; set; }
        string[] Scope { get; set; }
        string State { get; set; }
        bool IsApproved { get; set; }
        IToken Authorization { get; set; }
    }
}
