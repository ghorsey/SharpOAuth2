using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2.Provider
{
    public interface IOAuthContext
    {
        string[] Scope { get; set; }
        IClient Client { get; set; }
        Uri RedirectUri { get; set; }
    }
}
