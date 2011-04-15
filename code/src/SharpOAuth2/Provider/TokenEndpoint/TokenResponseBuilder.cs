using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using SharpOAuth2.Globalization;

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
        private string SerializeErrorResponse(IDictionary<string, object> response)
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
                errorResponse.Body = SerializeErrorResponse(errorResults);

                return errorResponse;
            }
            if (context.Token == null)
                throw new OAuthFatalException(TokenEndpointResources.ContextDoesNotContainToken);

            TokenResponse response = new TokenResponse { HttpStatusCode = 200 };
            response.Body = SerializeErrorResponse(context.Token.ToResponseValues());
            return response;
        }

        #endregion
    }
}
