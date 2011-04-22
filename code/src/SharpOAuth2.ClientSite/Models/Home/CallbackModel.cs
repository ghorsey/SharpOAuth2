
namespace SharpOAuth2.ClientSite.Models.Home
{
    public class CallbackModel
    {
        public bool HasError { get; private set; }
        public string Error { get; private set; }
        public string Description { get; private set; }
       
        public CallbackModel(bool hasError, string error, string description)
        {
            HasError = hasError;
            Error = error;
            Description = description;
        }
    }
}