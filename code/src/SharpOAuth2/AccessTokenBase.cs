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
            IssuedOn = DateTime.Now.ToEpoch();
            Parameters = new Dictionary<string, object>();
        }
        #region IToken Members

        public virtual string Token{ get; set; }
        public virtual string TokenType{ get; set; } 
        public virtual int ExpiresIn{ get; set; }
        public virtual string RefreshToken{ get; set; }
        public virtual long IssuedOn { get; set; }
        public virtual string[] Scope { get; set; }
        public virtual IDictionary<string, object> Parameters { get; private set; }

        public virtual IDictionary<string, object> ToResponseValues()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            dictionary[SharpOAuth2.Parameters.AccessToken] = Token;
            dictionary[SharpOAuth2.Parameters.AccessTokenExpiresIn] = ExpiresIn;
            dictionary[SharpOAuth2.Parameters.RefreshToken] = RefreshToken;
            dictionary[SharpOAuth2.Parameters.AccessTokenType] = TokenType;

            foreach (var itm in Parameters)
                dictionary.Add(itm.Key, itm.Value);

            return dictionary;
        }

        #endregion
    }
}
