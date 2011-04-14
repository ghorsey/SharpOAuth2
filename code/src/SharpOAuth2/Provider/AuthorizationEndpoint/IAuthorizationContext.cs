using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace SharpOAuth2.Provider.AuthorizationEndpoint
{
    public interface IAuthorizationContext : IOAuthContext
    {
        
        string ResponseType { get; set; }   
        string State { get; set; }
        bool IsApproved { get; set; }
        
        string ResourceOwnerId { get; set; }
    }
}
