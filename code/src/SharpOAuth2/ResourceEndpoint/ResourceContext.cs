#region License
/* The MIT License
 * 
 * Copyright (c) 2011 Geoff Horsey
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
#endregion

using System.Collections.Specialized;
using System.Text;
using SharpOAuth2.Framework;
using SharpOAuth2.Provider.Framework;

namespace SharpOAuth2.Provider.ResourceEndpoint
{
    public class ResourceContext : IResourceContext
    {
        #region IResourceContext Members
        public string Realm { get; set; }
        public ErrorResponse Error{ get; set; }
        public NameValueCollection Headers{ get; set; }
        public NameValueCollection QueryString{ get; set; }
        public NameValueCollection Form{ get; set; }
        public IToken Token{ get; set; }
        #endregion

        public override string ToString()
        {
            var builder = new StringBuilder();
            
            builder.Append("{");
            builder.AppendFormat(@"Realm: ""{0}"", ", SafeString(this.Realm));
            builder.AppendFormat(@"ErrorResponse: {0}, ", SafeString(this.Error));
            builder.AppendFormat(@"Headers: {0}, ", SafeString(this.Headers));
            builder.AppendFormat(@"QueryString: {0}, ", SafeString(this.QueryString));
            builder.AppendFormat(@"Form: {0}", SafeString(this.Form));
            builder.Append("}");

            return builder.ToString();
        }


        private static string SafeString(ErrorResponse error)
        {
            if (error == null) return "null";

            return error.ToString();
        }

        private static string SafeString(NameValueCollection coll)
        {
            if (coll == null) return "null";

            var builder = new StringBuilder();
            builder.Append("{");

            for (var i = 0; i < coll.Keys.Count; i++)
            {
                var format = @"{0}: ""{1}""";
                if (i != 0)
                    format = "," + format;
                var key = coll.Keys[i];

                builder.AppendFormat(format, key, coll[key]);
            }

            builder.Append("}");

            return builder.ToString();
        }

        private static string SafeString(string input)
        {
            if (input == null) return "";
            return input;
        }
    }
}
