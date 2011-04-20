
namespace SharpOAuthProvider.Domain.Repository
{
    public interface IClientRepository
    {
        Client FindClient(string clientId);
    }
}
