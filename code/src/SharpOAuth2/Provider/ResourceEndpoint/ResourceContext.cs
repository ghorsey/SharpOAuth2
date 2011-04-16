using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace SharpOAuth2.Provider.ResourceEndpoint
{
    public class ResourceContext : IResourceContext
    {
        #region IResourceContext Members

        public ErrorResponse Error{ get; set; }
        public NameValueCollection Headers{ get; set; }
        public NameValueCollection QueryString{ get; set; }
        public NameValueCollection Form{ get; set; }
        public IToken Token{ get; set; }
        #endregion
    }
}
