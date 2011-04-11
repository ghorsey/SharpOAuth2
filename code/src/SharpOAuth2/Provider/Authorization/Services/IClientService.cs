using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2.Provider.Authorization.Services
{
    /// <summary>
    /// Client service that or encapsulates that deals with the fetching and validating 
    /// clients <see cref="IClient" types/>
    /// </summary>
    public interface IClientService 
    {

        /// <summary>
        /// Authenticates the client.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// 	<c>true</c> if the client id and client secret match the values from the data store; otherwise <c>false</c>.
        /// </returns>
        bool AuthenticateClient(IAuthorizationContext context);
        /// <summary>
        /// Validates the redirect URI.
        /// </summary>
        /// <param name="context">The current authoricaton context<param>
        /// <returns><c>true</c> if the redirect uri matches the client record; otherwise, <c>false</c></returns>
        bool ValidateRedirectUri(IAuthorizationContext context);

        /// <summary>
        /// Determines whether the specified client is client.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// 	<c>true</c> if the specified client id matches an existing client; otherwise, <c>false</c>.
        /// </returns>
        bool IsClient(IAuthorizationContext context);
    }
}
