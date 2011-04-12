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

        public virtual string ToUriEncodedString()
        {
            string urlFormat = Parameters.AuthorizationResponse.Code + "={0}&" +
                Parameters.State + "={1}";

            return string.Format(CultureInfo.InvariantCulture, urlFormat, HttpUtility.UrlEncode(Token), HttpUtility.UrlEncode(string.Join(" ", Scope)));
        }

        public virtual string ToJsonEncodedString()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
