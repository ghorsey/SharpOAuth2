using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2
{
    public interface IClient
    {
        string ClientId { get; set; }
        string ClientSecret { get; set; }
    }
}
