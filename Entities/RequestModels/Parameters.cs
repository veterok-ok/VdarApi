namespace Entities.RequestModels
{
    public class Parameters : ClientParameters
    {
        public string grant_type { get; set; }
        public string refresh_token { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}
