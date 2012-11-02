using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2.Provider.Domain
{
	public interface IAuthorizationGrant
	{
		string Code { get; set; }
	}
}
