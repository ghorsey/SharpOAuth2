using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2
{
    public class ClientBase : IClient
    {
        #region IClient Members

        public string ClientId{ get; set; }
        public string ClientSecret{ get; set; }

        #endregion
    }
}
