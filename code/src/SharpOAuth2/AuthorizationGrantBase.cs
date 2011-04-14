using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Globalization;

namespace SharpOAuth2
{
    public class AuthorizationGrantBase : IToken
    {
        #region IToken Members

        public virtual string Token { get; set; }
        public virtual string TokenType { get; set; }
        public virtual int Expires { get; set; }
        public virtual string RefreshToken { get; set; }
        public virtual string[] Scope { get; set; }
        #endregion
    }
}
