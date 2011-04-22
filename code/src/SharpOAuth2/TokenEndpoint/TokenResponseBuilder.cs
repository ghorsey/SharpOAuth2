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

using System.Collections.Generic;
using Newtonsoft.Json;
using SharpOAuth2.Framework;
using SharpOAuth2.Provider.Framework;
using SharpOAuth2.Provider.Globalization;

namespace SharpOAuth2.Provider.TokenEndpoint
{
    public class TokenResponseBuilder : ITokenResponseBuilder
    {
        private IDictionary<string, object> RemoveEmptyOrNullResponseValues(IDictionary<string, object> toClean)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            foreach (string key in toClean.Keys)
            {
                if (toClean[key] == null) continue;
                if (toClean[key] is string && string.IsNullOrWhiteSpace(toClean[key] as string)) continue;
                result[key] = toClean[key];
            }
            return result;
        }
        private string SerializeResponse(IDictionary<string, object> response)
        {
            IDictionary<string, object> toSerialize = RemoveEmptyOrNullResponseValues(response);

            return JsonConvert.SerializeObject(toSerialize);
        }

        #region ITokenResponseBuilder Members

        public TokenResponse CreateResponse(ITokenContext context)
        {
            if (context.Error != null)
            {
                TokenResponse errorResponse = new TokenResponse();
                errorResponse.HttpStatusCode = 400;
                IDictionary<string, object> errorResults = context.Error.ToResponseValues();
                if (context.Error.Error == Parameters.ErrorParameters.ErrorValues.InvalidClient)
                    errorResponse.HttpStatusCode = 401;
                errorResponse.Body = SerializeResponse(errorResults);

                return errorResponse;
            }
            if (context.Token == null)
                throw new OAuthFatalException(TokenEndpointResources.ContextDoesNotContainToken);

            TokenResponse response = new TokenResponse { HttpStatusCode = 200 };
            response.Body = SerializeResponse(((ITokenizer)context.Token).ToResponseValues());
            return response;
        }

        #endregion
    }
}
