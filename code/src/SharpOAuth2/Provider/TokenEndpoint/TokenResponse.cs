using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2.Provider.TokenEndpoint
{
    public class TokenResponse
    {
        public int HttpStatusCode { get; set; }
        public string ContentType { get; private set; }
        public string Body { get; set; }

        public TokenResponse()
        {
            ContentType = "application/json";
        }
    }
}
