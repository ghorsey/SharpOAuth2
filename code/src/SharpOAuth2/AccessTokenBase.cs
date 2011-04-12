using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2
{
    public class AccessTokenBase : IToken
    {
        #region IToken Members

        public virtual string Token{ get; set; }
        public virtual string TokenType{ get; set; } 
        public virtual int Expires{ get; set; }
        public virtual string RefreshToken{ get; set; }
        public virtual string[] Scope{ get; set; }
       

        public virtual string ToUriEncodedString()
        {
            throw new NotImplementedException();
        }

        public virtual string ToJsonEncodedString()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
