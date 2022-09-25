namespace TripYatri.Core.API.Auth
{
    public static class AuthConstants
    {
        public const string AuthorizationHeader = "Authorization";
        public const string X3ScaleHeader = "X-3scale-proxy-secret-token";
        public const string XProxyHeader = "X-proxy-secret-token";
        public const string XForwardedFor = "X-Forwarded-For";
    }
}
