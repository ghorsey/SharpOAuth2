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
    }
}
