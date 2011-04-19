using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOAuth2.Provider.AuthorizationEndpoint;

namespace SharpOAuth2.Provider.Services
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
        bool AuthenticateClient(IOAuthContext context);
        /// <summary>
        /// Validates the redirect URI.
        /// </summary>
        /// <param name="context">The current authoricaton context<param>
        /// <returns><c>true</c> if the redirect uri matches the client record; otherwise, <c>false</c></returns>
        bool ValidateRedirectUri(IOAuthContext context);

        /// <summary>
        /// Determines whether the specified client is client.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// 	<c>true</c> if the specified client id matches an existing client; otherwise, <c>false</c>.
        /// </returns>
        bool IsClient(IOAuthContext context);

        /// <summary>
        /// Finds the client.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <returns></returns>
        ClientBase FindClient(string clientId);

        /// <summary>
        /// Determines whether the resource owner has granted the client the specified scope.
        /// </summary>
        /// <param name="client">The client requesting access.</param>
        /// <param name="scope">The scope the client is requesting.</param>
        /// <param name="resourceOwnerId">The resource owner that the client is requesting access from.</param>
        /// <returns>
        /// 	<c>true</c> if the access is granted for the specified client; otherwise, <c>false</c>.
        /// </returns>
        bool IsAccessGranted(IClient client, string[] scope, string resourceOwnerId);
    }
}
