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
using System.Collections.Specialized;
using System.Web;
using SharpOAuth2.Framework;
using SharpOAuth2.Framework.Utility;
using SharpOAuth2.Provider.Framework;

namespace SharpOAuth2.Provider.AuthorizationEndpoint
{
    public class AuthorizationResponseBuilder : IAuthorizationResponseBuilder
    {
        

        #region IAuthorizationResponseBuilder Members

        public Uri CreateResponse(IAuthorizationContext context)
        {

            UriBuilder result = new UriBuilder(context.RedirectUri);

            NameValueCollection queryComponents = GetContextToModify(context);
            
            if (context.Error != null)
            {
                BuildResponseValues(queryComponents, context.Error.ToResponseValues());

                SetModifiedContext(context, result, queryComponents);

                return result.Uri;
            }

            IDictionary<string, object> responseValues = ((ITokenizer)context.Token).ToResponseValues();
            BuildResponseValues(queryComponents, responseValues);
            queryComponents[Parameters.State] = context.State;

            SetModifiedContext(context, result, queryComponents);

            return result.Uri;
            
        }

        private static void BuildResponseValues(NameValueCollection queryComponents, IDictionary<string, object> responseValues)
        {
            foreach (string key in responseValues.Keys)
            {
                if (responseValues[key] == null) continue;
                if (responseValues[key] is string && string.IsNullOrWhiteSpace(responseValues[key] as string)) continue;

                queryComponents[key] = responseValues[key].ToString();
            }
        }

        private void SetModifiedContext(IAuthorizationContext context, UriBuilder builder, NameValueCollection components)
        {
            if (context.ResponseType == Parameters.ResponseTypeValues.AccessToken)
                builder.Fragment = UriHelper.ReconstructQueryString(components);
            else
                builder.Query = UriHelper.ReconstructQueryString(components);
        }

        private NameValueCollection GetContextToModify(IAuthorizationContext context)
        {
            if (context.ResponseType == Parameters.ResponseTypeValues.AccessToken)
                return new NameValueCollection();
            else
                return HttpUtility.ParseQueryString(context.RedirectUri.Query);
        }
        #endregion
    }
}
