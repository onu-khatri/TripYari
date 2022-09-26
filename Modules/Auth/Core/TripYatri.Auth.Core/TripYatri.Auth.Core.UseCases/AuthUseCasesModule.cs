using Autofac;
using FluentValidation;
using MediatR;
using TripYari.Auth.Core.UseCases.RegisterUser;
using TripYari.Auth.Core.UseCases.Token;

namespace TripYari.Auth.Core.UseCases
{
    public class AuthUseCasesModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<AddUserValidator>()
                .As<IValidator<AddUserRequest>>();

            builder.RegisterType<AddUserInteractor>()
                .As<IRequestHandler<AddUserRequest, AddUserResponse>>();

            builder.RegisterType<GetTokenInteractor>()
                .As<IRequestHandler<GetTokenRequest, GetTokenResponse>>();
            builder.RegisterType<TokenFactory>()
                .As<ITokenFactory>();
            builder.RegisterType<TokenValidator>()
                .As<ITokenValidator>();

           }

    }
}
