﻿using System.Collections.Immutable;
using System.Text;
using FluentValidation;
using LocalIdentity.SimpleInfra.Application.Common.Identity.Models;
using LocalIdentity.SimpleInfra.Application.Common.Identity.Services;
using LocalIdentity.SimpleInfra.Domain.Entities;
using LocalIdentity.SimpleInfra.Domain.Extensions;
using LocalIdentity.SimpleInfra.Infrastructure.Common.Settings;
using Microsoft.Extensions.Options;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Identity.Services;

public class PasswordGeneratorService(
    IOptions<PasswordValidationSettings> passwordValidationSettings,
    IValidator<CredentialDetails> credentialDetailsValidator
) : IPasswordGeneratorService
{
    private readonly PasswordValidationSettings _passwordValidationSettings = passwordValidationSettings.Value;
    private readonly Random _random = new();

    private enum PasswordElementType
    {
        Digit = 0,
        Uppercase = 1,
        Lowercase = 2,
        NonAlphanumeric = 3
    }

    public string GeneratePassword()
    {
        var password = new StringBuilder();

        var requiredElements = GetRequiredElements();
        requiredElements.ForEach(element => password.Append(GetPasswordElement(element)));

        while (password.Length < _passwordValidationSettings.MinimumLength)
            password.Append(GetPasswordElement((PasswordElementType)_random.Next(0, requiredElements.Count - 1)));

        var randomizedPassword = password.ToString().ToCharArray();
        _random.Shuffle(randomizedPassword);
        return new string(randomizedPassword);
    }

    public string GetValidatedPassword(string password, User user)
    {
        var validationContext = new ValidationContext<CredentialDetails>(new CredentialDetails
        {
            Password = password
        })
        {
            RootContextData =
            {
                ["PersonalInformation"] = new[] { user.FirstName, user.LastName, user.EmailAddress }
            }
        };

        var validationResult = credentialDetailsValidator.Validate(validationContext);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return password;
    }

    private ImmutableList<PasswordElementType> GetRequiredElements()
    {
        var requiredElements = new List<PasswordElementType>();

        if (_passwordValidationSettings.RequireDigit)
            requiredElements.Add(PasswordElementType.Digit);

        if (_passwordValidationSettings.RequireUppercase)
            requiredElements.Add(PasswordElementType.Uppercase);

        if (_passwordValidationSettings.RequireLowercase)
            requiredElements.Add(PasswordElementType.Lowercase);

        if (_passwordValidationSettings.RequireNonAlphanumeric)
            requiredElements.Add(PasswordElementType.NonAlphanumeric);

        return requiredElements.ToImmutableList();
    }

    private char GetPasswordElement(PasswordElementType passwordElementType)
    {
        return passwordElementType switch
        {
            PasswordElementType.Digit => CharExtensions.GetRandomDigit(_random),
            PasswordElementType.Uppercase => CharExtensions.GetRandomUppercase(_random),
            PasswordElementType.Lowercase => CharExtensions.GetRandomLowercase(_random),
            PasswordElementType.NonAlphanumeric => CharExtensions.GetRandomNonAlphanumeric(_random),
            _ => throw new ArgumentOutOfRangeException(nameof(passwordElementType), passwordElementType, null)
        };
    }
}