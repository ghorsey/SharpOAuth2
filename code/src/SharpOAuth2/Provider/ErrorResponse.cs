using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2.Provider
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
    }
}
