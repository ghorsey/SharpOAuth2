using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2.Provider.TokenEndpoint
{
    public class TokenContextBuilder : IContextBuilder<ITokenContext>
    {
        #region IContextBuilder<ITokenContext> Members

        public ITokenContext FromUri(string url)
        {
            return FromUri(new Uri(url, UriKind.Absolute));
        }

        public ITokenContext FromUri(Uri uri)
        {
            throw new NotImplementedException();
        }

        public ITokenContext FromHttpRequest(System.Web.HttpRequestBase request)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
