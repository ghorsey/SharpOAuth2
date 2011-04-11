using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2
{
    public interface IToken 
    {
        string Token { get; set; }
        string TokenType { get; set; } // bearer, mac
        int Expires{ get; set; }
        string RefreshToken{ get; set; }
        string[] Scope { get; set; }
    }
}
