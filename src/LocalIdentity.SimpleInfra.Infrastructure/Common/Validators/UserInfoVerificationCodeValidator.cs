using FluentValidation;
using LocalIdentity.SimpleInfra.Domain.Entities;
using LocalIdentity.SimpleInfra.Domain.Enums;
using LocalIdentity.SimpleInfra.Infrastructure.Common.Settings;
using Microsoft.Extensions.Options;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Validators;

public class UserInfoVerificationCodeValidator : AbstractValidator<UserInfoVerificationCode>
{
    public UserInfoVerificationCodeValidator(IOptions<VerificationSettings> verificationSettings, IOptions<ValidationSettings> validationSettings)
    {
        var verificationSettingsValue = verificationSettings.Value;
        var validationSettingsValue = validationSettings.Value;

        RuleSet(
            EntityEvent.OnCreate.ToString(),
            () =>
            {
                RuleFor(code => code.UserId).NotEqual(Guid.Empty);

                RuleFor(code => code.ExpiryTime)
                    .GreaterThanOrEqualTo(DateTimeOffset.UtcNow)
                    .LessThanOrEqualTo(DateTimeOffset.UtcNow.AddSeconds(verificationSettingsValue.VerificationCodeExpiryTimeInSeconds));

                RuleFor(code => code.IsActive).Equal(true);

                RuleFor(code => code.VerificationLink).NotEmpty().Matches(validationSettingsValue.UrlRegexPattern);
            }
        );
    }
}