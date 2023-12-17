using FluentValidation;
using LocalIdentity.SimpleInfra.Application.Common.Identity.Models;
using LocalIdentity.SimpleInfra.Infrastructure.Common.Settings;
using Microsoft.Extensions.Options;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Validators;

public class SignUpDetailsValidator : AbstractValidator<SignUpDetails>
{
    public SignUpDetailsValidator(IOptions<ValidationSettings> validationSettings, IOptions<PasswordValidationSettings> passwordValidationSettings)
    {
        var validationSettingsValue = validationSettings.Value;
        var passwordValidationSettingsValue = passwordValidationSettings.Value;

        RuleFor(signUpDetails => signUpDetails.EmailAddress)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(64)
            .Matches(validationSettingsValue.EmailAddressRegexPattern);

        RuleFor(signUpDetails => signUpDetails.FirstName)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(64)
            .Matches(validationSettingsValue.NameRegexPattern)
            .WithMessage("First name is not valid");

        RuleFor(signUpDetails => signUpDetails.LastName)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(64)
            .Matches(validationSettingsValue.NameRegexPattern)
            .WithMessage("Last name is not valid");

        RuleFor(signUpDetails => signUpDetails.Age).GreaterThanOrEqualTo(18).LessThanOrEqualTo(130);

        RuleFor(signUpDetails => signUpDetails.Password)
            .NotNull()
            .WithMessage("Password is required.")
            .MinimumLength(passwordValidationSettingsValue.MinimumLength)
            .WithMessage($"Password must be at least {passwordValidationSettingsValue.MinimumLength} characters long.")
            .MaximumLength(passwordValidationSettingsValue.MaximumLength)
            .WithMessage($"Password must be at most {passwordValidationSettingsValue.MaximumLength} characters long.")
            .Custom(
                (password, context) =>
                {
                    if (passwordValidationSettingsValue.RequireDigit && !password.Any(char.IsDigit))
                        context.AddFailure("Password must contain at least one digit.");
                }
            )
            .Custom(
                (password, context) =>
                {
                    if (passwordValidationSettingsValue.RequireUppercase && !password.Any(char.IsUpper))
                        context.AddFailure("Password must contain at least one uppercase letter.");
                }
            )
            .Custom(
                (password, context) =>
                {
                    if (passwordValidationSettingsValue.RequireLowercase && !password.Any(char.IsLower))
                        context.AddFailure("Password must contain at least one lowercase letter.");
                }
            )
            .Custom(
                (password, context) =>
                {
                    if (passwordValidationSettingsValue.RequireNonAlphanumeric && password.All(char.IsLetterOrDigit))
                        context.AddFailure("Password must contain at least one non-alphanumeric character.");
                }
            )
            .Custom(
                (password, context) =>
                {
                    if (passwordValidationSettingsValue.RequireNonAlphanumeric && password.All(char.IsLetterOrDigit))
                        context.AddFailure("Password must contain at least one non-alphanumeric character.");
                }
            )
            .When(signUpDetails => !signUpDetails.AutoGeneratePassword);
    }
}