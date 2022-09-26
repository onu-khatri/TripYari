using TripYari.Auth.Core.UseCases.Models;

namespace TripYari.Auth.Core.UseCases.RegisterUser
{
    public class AddUserResponse : JwtTokensData
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string EmailConfirmationToken { get; set; }
    }
}
