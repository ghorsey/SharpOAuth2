using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2
{
    public class AccessTokenMacBase : AccessTokenBase
    {
        public string TokenSecret { get; set; }
        public int Timestamp { get; set; }
        public string Nounce { get; set; }
        public string BodyHash { get; set; }
        public string Signature { get; set; }

    }
}
