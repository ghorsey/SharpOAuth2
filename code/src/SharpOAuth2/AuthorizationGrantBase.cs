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
        public virtual int ExpiresIn { get; set; }
        public virtual string RefreshToken { get; set; }
        public virtual string[] Scope { get; set; }
        public virtual long IssuedOn { get; private set; }
        public virtual bool IsApproved { get; set; }
        public virtual ClientBase Client { get; set; }
        public virtual Uri RedirectUri { get; set; }
        public virtual IDictionary<string, object> Parameters { get; private set; }
        #endregion

        public AuthorizationGrantBase()
        {
            IssuedOn = DateTime.Now.ToEpoch();
            Parameters = new Dictionary<string, object>();
        }

        #region IToken Members


        public IDictionary<string, object> ToResponseValues()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters[SharpOAuth2.Parameters.AuthroizationCode] = Token;


            foreach (var item in Parameters)
                parameters.Add(item.Key, item.Value);

            return parameters;
        }

        #endregion
    }
}
