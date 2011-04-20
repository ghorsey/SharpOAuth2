
namespace SharpOAuth2.ClientSite.Models.Home
{
    public class CallbackModel
    {
        public bool HasError { get; private set; }
        public string Error { get; private set; }
        public string Description { get; private set; }
        public string TokenResponse { get; private set; }
        public CallbackModel(string token, bool hasError, string error, string description)
        {
            TokenResponse = token;
            HasError = hasError;
            Error = error;
            Description = description;
        }
    }
}