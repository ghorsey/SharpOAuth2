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

using System;
using System.Collections.Generic;
using System.Text;
using SharpOAuth2.Framework;

namespace SharpOAuth2.Provider.Framework
{
    public class ErrorResponse
    {
        public string Error { get; set; }
        public string ErrorDescription { get; set; }
        public Uri ErrorUri { get; set; }
        public IDictionary<string, object> ToResponseValues()
        {
            Dictionary<string, object> response = new Dictionary<string, object>();

            response[Parameters.ErrorParameters.Error] = Error;
            response[Parameters.ErrorParameters.ErrorDescription] = ErrorDescription;
            response[Parameters.ErrorParameters.ErrorUri] = ErrorUri;

            return response;
        }

        private static string SafeString(Uri input)
        {
            if (input == null) return "";
            return input.ToString();
        }
        private static string SafeString(string input)
        {
            if (input == null) return "";
            return input;
        }
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("{");
            builder.AppendFormat(@"Error: ""{0}"", ", SafeString(Error));
            builder.AppendFormat(@"ErrorDescription: ""{0}"", ", SafeString(ErrorDescription));
            builder.AppendFormat(@"ErrorUri: ""{0}""", SafeString(ErrorUri));
            builder.Append("}");
            return base.ToString();
        }
    }
}
