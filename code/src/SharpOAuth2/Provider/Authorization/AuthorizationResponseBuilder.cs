using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Web;
using System.IO;

namespace SharpOAuth2.Provider.Authorization
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
                queryComponents[Parameters.ErrorParameters.Error] = context.Error.Error;
                queryComponents[Parameters.ErrorParameters.ErrorDescription] = context.Error.ErrorDescription;
                queryComponents[Parameters.ErrorParameters.ErrorUri] = context.Error.ErrorUri != null ? context.Error.ErrorUri.ToString() : string.Empty;

                SetModifiedContext(context, result, queryComponents);

                return result.Uri;
            }
            result.Fragment = "monkey=butter";
            queryComponents[Parameters.AuthroizationCode] = context.Authorization.Token;
            queryComponents[Parameters.State] = context.State;

            SetModifiedContext(context, result, queryComponents);

            return result.Uri;
            
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
