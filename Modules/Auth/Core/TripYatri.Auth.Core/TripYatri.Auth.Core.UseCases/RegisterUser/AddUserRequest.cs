using FluentValidation;
using MediatR;
using TripYari.Auth.Core.UseCases.Enum;
using System.ComponentModel.DataAnnotations;
using System;

namespace TripYari.Auth.Core.UseCases.RegisterUser
{
    public class AddUserRequest : IRequest<AddUserResponse>
    {        
        [EmailAddress]
        public string Email { get; set; }
        public Guid Id { get; set; }
        [Required]
        public string Password { get; set; }

        public UserType UserType { get; set; } = UserType.User;

        public string Mobile { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class AddUserValidator : AbstractValidator<AddUserRequest>
    {
        public AddUserValidator()
        {
            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .WithMessage("Email could not be null or empty.")
                .Must(x => !string.IsNullOrEmpty(x) && x.Length <= 20)
                .WithMessage("Email should be between 1 and 20 chars.");

            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty()
                .WithMessage("Password could not be null or empty.")
                .Must(x => !string.IsNullOrEmpty(x) && x.Length >= 8 && x.Length <= 16)
                .WithMessage("Password should be between 8 and 16 chars.");
        }
    }
}
