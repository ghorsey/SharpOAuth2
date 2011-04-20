using System;
using SharpOAuth2.Provider.Domain;

namespace SharpOAuthProvider.Domain
{
    public class Client : ClientBase
    {
        public Uri RedirectUri { get; set; }
        public string Name { get; set; }
    }
}
