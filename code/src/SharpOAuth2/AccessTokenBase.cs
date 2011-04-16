using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2
{
    public class AccessTokenBase : IToken
    {

        public AccessTokenBase()
        {
            Created = DateTime.Now.ToEpoch();
        }
        #region IToken Members

        public virtual string Token{ get; set; }
        public virtual string TokenType{ get; set; } 
        public virtual int ExpiresIn{ get; set; }
        public virtual string RefreshToken{ get; set; }
        public virtual long Created { get; private set; }

        public virtual IDictionary<string, object> ToResponseValues()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            dictionary[Parameters.AccessToken] = Token;
            dictionary[Parameters.AccessTokenExpiresIn] = ExpiresIn;
            dictionary[Parameters.RefreshToken] = RefreshToken;
            dictionary[Parameters.AccessTokenType] = TokenType;

            return dictionary;
        }

        #endregion
    }
}
