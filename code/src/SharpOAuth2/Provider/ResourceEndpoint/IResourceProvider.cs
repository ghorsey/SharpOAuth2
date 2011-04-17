using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2.Provider.ResourceEndpoint
{
    public interface IResourceProvider
    {
        void AccessProtectedResource(IResourceContext context);
    }
}
