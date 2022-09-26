namespace TripYari.Auth.Core.UseCases.Models
{
    public class JwtTokensData
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string RefreshTokenSerial { get; set; }
        public string UserName { get; set; }
    }
}
