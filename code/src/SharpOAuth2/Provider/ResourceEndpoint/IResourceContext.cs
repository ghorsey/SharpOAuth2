using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace SharpOAuth2.Provider.ResourceEndpoint
{
    public interface IResourceContext
    {
        ErrorResponse Error { get; set; }
        NameValueCollection Headers { get; set; }
        NameValueCollection QueryString { get; set; }
        NameValueCollection Form { get; set; }
        string Realm { get; set; }

        IToken Token { get; set; }
    }
}
