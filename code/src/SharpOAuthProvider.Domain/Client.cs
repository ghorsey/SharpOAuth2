using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOAuth2;

namespace SharpOAuthProvider.Domain
{
    public class Client : ClientBase
    {
        public Uri RedirectUri { get; set; }
    }
}
