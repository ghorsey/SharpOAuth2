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

namespace SharpOAuth2.Provider.Framework
{
    [Serializable]
    public class OAuthErrorResponseException<T> : Exception where T : class
    {
        public T Context{ get; private set; }
        public string Error { get; private set; }
        public Uri ErrorUri { get; private set; }
        public int HttpStatusCode { get; private set; }

        public OAuthErrorResponseException(T context, string error)
            : this()
        {
            Context = context;
            Error = error;
            HttpStatusCode = 400;
        }

        public OAuthErrorResponseException(T context, string error, string description = "", int httpStatusCode = 400, Uri errorUri = null)
            : this( description )
        {
            ErrorUri = errorUri;
            Error = error;
            Context = context;
            HttpStatusCode = httpStatusCode;
        }
        public OAuthErrorResponseException(T context, string error, string description, Exception inner, Uri errorUri = null)
            : this(description, inner)
        {
            ErrorUri = errorUri;
            Error = error;
            Context = context;
            HttpStatusCode = 400;
        }

        private OAuthErrorResponseException() { HttpStatusCode = 400; }
        private OAuthErrorResponseException(string message) : base(message) { HttpStatusCode = 400; }
        private OAuthErrorResponseException(string message, Exception inner) : base(message, inner) { HttpStatusCode = 400; }
        protected OAuthErrorResponseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { HttpStatusCode = 400; }
    }
}
