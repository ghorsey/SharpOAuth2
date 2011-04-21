using System;
using System.Collections.Generic;

namespace SharpOAuthProvider.Domain.Repository
{
    public class InMemoryClientRepository : IClientRepository
    {
        static readonly IDictionary<string, Client> _clients = new Dictionary<string, Client>();

        static InMemoryClientRepository()
        {
            _clients.Add("12345", new Client { Name="Sample Client", ClientId = "12345", ClientSecret = "secret", RedirectUri = new Uri("http://localhost:15075/Home/Callback", UriKind.Absolute) });
        }

        #region IClientRepository Members

        public Client FindClient(string clientId)
        {
            if (!_clients.ContainsKey(clientId)) return null;

            return _clients[clientId];
        }

        #endregion
    }
}
