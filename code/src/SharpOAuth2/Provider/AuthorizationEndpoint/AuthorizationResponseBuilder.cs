using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Web;
using System.IO;

namespace SharpOAuth2.Provider.AuthorizationEndpoint
{
    public class AuthorizationResponseBuilder : IAuthorizationResponseBuilder
    {
        private string ReconstructQuery(NameValueCollection queryComponents)
        {
            StringBuilder output = new StringBuilder();
            TextWriter writer = new StringWriter(output);
           
            string queryComponentFormat = "{0}={1}";
            for (int i = 0; i < queryComponents.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(queryComponents[i])) continue; // ignore blank or empty

                if (i > 0)
                    writer.Write("&");

                writer.Write(queryComponentFormat, queryComponents.Keys[i], Uri.EscapeDataString(queryComponents[i]));   
            }

            return output.ToString();
        }

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

            IDictionary<string, object> responseValues = context.Token.ToResponseValues();
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
                if (string.IsNullOrWhiteSpace(responseValues[key] as string)) continue;

                queryComponents[key] = responseValues[key].ToString();
            }
        }

        private void SetModifiedContext(IAuthorizationContext context, UriBuilder builder, NameValueCollection components)
        {
            if (context.ResponseType == Parameters.ResponseTypeValues.AccessToken)
                builder.Fragment = ReconstructQuery(components);
            else
                builder.Query = ReconstructQuery(components);
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
