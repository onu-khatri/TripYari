using MediatR;
using System;
using TripYari.Auth.Core.UseCases.Enum;

namespace TripYari.Auth.Core.UseCases.Token
{
    public class GetTokenRequest: IRequest<GetTokenResponse>
    {
        public Guid Id { get; set; }
        public string Password { get; set; }
        public UserType UserRole { get; set; }
    }
}
