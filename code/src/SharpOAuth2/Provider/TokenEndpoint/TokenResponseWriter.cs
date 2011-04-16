using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SharpOAuth2.Provider.TokenEndpoint
{
    public class TokenResponseWriter : TokenWriter<HttpResponseBase>
    {
        public TokenResponseWriter(HttpResponseBase response) : base(response) { }

        #region ITokenWriter Members

        public override void WriteResponse(TokenResponse tokenResponse)
        {
            Destination.AddHeader("Cache-Control", "no-store");
            Destination.ContentType = tokenResponse.ContentType;
            Destination.StatusCode = tokenResponse.HttpStatusCode;
            Destination.Write(tokenResponse.Body);
        }

        #endregion
    }
}
