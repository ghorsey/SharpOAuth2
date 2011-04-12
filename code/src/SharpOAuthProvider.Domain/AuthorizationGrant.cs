using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOAuth2;

namespace SharpOAuthProvider.Domain
{
    public class AuthorizationGrant : AuthorizationGrantBase
    {
        public bool IsUsed { get; set; }
        public Client Client { get; set; }

        public string ResourceOwnerId { get; set; }
        public DateTime Created { get; private set; }
        public bool IsApproved { get; set; }
        public AuthorizationGrant()
        {
            Created = DateTime.Now;
        }
    }
}
