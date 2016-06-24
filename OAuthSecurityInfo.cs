using System;

namespace OAuthProxy
{
    public class OAuthSecurityInfo
    {
        public string Authority { get; set; }
        public string Resource { get; set; }
        public string ClientId { get; set; }
        public Uri RedirectUri { get; set; }
    }
}