using TripYari.Auth.Core.UseCases.Models;

namespace TripYari.Auth.Core.UseCases.Token
{
    public class GetTokenResponse: JwtTokensData
    {
        public string FailureMessage { get; set; }
        public bool Status { get; set; }
    }
}
