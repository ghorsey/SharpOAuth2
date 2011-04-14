using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;

namespace SharpOAuth2.Provider.AuthorizationEndpoint
{
    public interface IAuthorizationContextBuilder
    {
        IAuthorizationContext FromUri(string url);
        IAuthorizationContext FromUri(Uri uri);
        IAuthorizationContext FromHttpRequest(HttpRequestBase request);
    }
}
