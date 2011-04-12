using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuthProvider.Domain.Repository
{
    public class InMemoryClientRepository : IClientRepository
    {
        static readonly IDictionary<string, Client> _clients;

        static InMemoryClientRepository()
        {
            _clients = new Dictionary<string, Client>();
            _clients.Add("12345", new Client { Name="Sample Provider", ClientId = "12345", ClientSecret = "secret", RedirectUri = new Uri("http://localhost:15075/Home/Callback", UriKind.Absolute) });
        }

        #region IClientRepository Members

        public Client LoadClient(string clientId)
        {
            if (!_clients.ContainsKey(clientId)) return null;

            return _clients[clientId];
        }

        #endregion
    }
}
