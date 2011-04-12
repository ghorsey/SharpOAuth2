using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuthProvider.Domain.Repository
{
    public interface IClientRepository
    {
        Client LoadClient(string clientId);
    }
}
