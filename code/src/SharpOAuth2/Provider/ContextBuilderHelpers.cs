using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2.Provider
{
    internal static class ContextBuilderHelpers
    {
        public static Uri CreateRedirectUri(string uri)
        {
            if (string.IsNullOrWhiteSpace(uri)) return null;
            return new Uri(uri, UriKind.Absolute);
        }

        public static string[] CreateScope(string scope)
        {
            if (string.IsNullOrWhiteSpace(scope)) return new string[0];

            return scope.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
