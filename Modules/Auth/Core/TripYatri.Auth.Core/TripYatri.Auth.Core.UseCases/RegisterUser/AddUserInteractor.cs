using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using TripYari.Auth.Core.DataContracts.Entities;
using System.Threading;
using System.Threading.Tasks;
using TripYari.Auth.Core.DataContracts.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using TripYari.Auth.Core.UseCases.Token;

namespace TripYari.Auth.Core.UseCases.RegisterUser
{
    public class AddUserInteractor : IRequestHandler<AddUserRequest, AddUserResponse>
    {
        private readonly IAppUserManager<AppIdentityUser> _userManager;
        private readonly IAppRoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IValidator<AddUserRequest> _addUserValidator;
        private readonly AppSettings _appSettings;
        private readonly ITokenFactory _tokenFactory;

        public AddUserInteractor(IAppUserManager<AppIdentityUser> userManager, IAppRoleManager<IdentityRole<Guid>> roleManager, IOptions<AppSettings> appSettings, IValidator<AddUserRequest> addUserValidator, ITokenFactory tokenFactory)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _addUserValidator = addUserValidator;
            _appSettings = appSettings.Value;
            _tokenFactory = tokenFactory;
        }

        public async Task<AddUserResponse> Handle(AddUserRequest request, CancellationToken cancellationToken)
        {
            await _addUserValidator.ValidateAsync(request);

            var existUser = await _userManager.FindByIdAsync(request.Id.ToString());
            if (existUser != null)
            {
                return new AddUserResponse { Status = false, Message = "User already exists." };
            }

            var user = new AppIdentityUser { UserName = request.Mobile, Email = request.Email, IsApproved = true, LockoutEnabled = true, TwoFactorEnabled = false, Id = request.Id };

            if (!await _roleManager.RoleExistsAsync(request.UserType.ToString()))
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole<Guid>
                {
                    Id = Guid.NewGuid(),
                    Name = request.UserType.ToString(),
                    NormalizedName = request.UserType.ToString()
                });

                if (!roleResult.Succeeded)
                {
                    return new AddUserResponse { Status = false, Message = "Fail to assign role." };
                }
            }

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, request.UserType.ToString());

                if (roleResult.Succeeded)
                {
                    var tokenData = _tokenFactory.CreateJwtTokens(user, _appSettings, request.UserType.ToString());
                    return new AddUserResponse
                    {
                        Status = true,
                        Message = "Registered Successfully",
                        EmailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user),
                        AccessToken = tokenData.AccessToken,
                        RefreshToken = tokenData.RefreshToken,
                        RefreshTokenSerial = tokenData.RefreshTokenSerial,
                        UserName = user.UserName
                    };
                }

            }

            return new AddUserResponse { Status = false, };
        }
    }
}
