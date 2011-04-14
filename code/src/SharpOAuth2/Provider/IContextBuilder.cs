using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SharpOAuth2.Provider
{
    public interface IContextBuilder<T> where T : IOAuthContext
    {
        T FromUri(string url);
        T FromUri(Uri uri);
        T FromHttpRequest(HttpRequestBase request);
    }
}
