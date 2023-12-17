using System.Text;
using System.Text.RegularExpressions;
using FluentValidation;
using LocalIdentity.SimpleInfra.Application.Common.Notifications.Models;
using LocalIdentity.SimpleInfra.Application.Common.Notifications.Services;
using LocalIdentity.SimpleInfra.Domain.Enums;
using LocalIdentity.SimpleInfra.Infrastructure.Common.Settings;
using Microsoft.Extensions.Options;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Notifications.Services;

public class EmailRenderingService : IEmailRenderingService
{
    private readonly IValidator<EmailMessage> _emailMessageValidator;
    private readonly TemplateRenderingSettings _templateRenderingSettings;

    public EmailRenderingService(IOptions<TemplateRenderingSettings> templateRenderingSettings, IValidator<EmailMessage> emailMessageValidator)
    {
        _templateRenderingSettings = templateRenderingSettings.Value;
        _emailMessageValidator = emailMessageValidator;
    }

    public ValueTask<string> RenderAsync(EmailMessage emailMessage, CancellationToken cancellationToken = default)
    {
        var validationResult = _emailMessageValidator.Validate(
            emailMessage,
            options => options.IncludeRuleSets(NotificationProcessingEvent.OnRendering.ToString())
        );
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

        var placeholderRegex = new Regex(
            _templateRenderingSettings.PlaceholderRegexPattern,
            RegexOptions.Compiled,
            TimeSpan.FromSeconds(_templateRenderingSettings.RegexMatchTimeoutInSeconds)
        );

        var placeholderValueRegex = new Regex(
            _templateRenderingSettings.PlaceholderValueRegexPattern,
            RegexOptions.Compiled,
            TimeSpan.FromSeconds(_templateRenderingSettings.RegexMatchTimeoutInSeconds)
        );

        var matches = placeholderRegex.Matches(emailMessage.EmailTemplate.Content);

        if (matches.Any() && !emailMessage.Variables.Any())
            throw new InvalidOperationException("Variables are required for this template.");

        var templatePlaceholders = matches.Select(
                match =>
                {
                    var placeholder = match.Value;
                    var placeholderValue = placeholderValueRegex.Match(placeholder).Groups[1].Value;
                    var valid = emailMessage.Variables.TryGetValue(placeholderValue, out var value);

                    return new TemplatePlaceholder
                    {
                        Placeholder = placeholder,
                        PlaceholderValue = placeholderValue,
                        Value = value,
                        IsValid = valid
                    };
                }
            )
            .ToList();

        ValidatePlaceholders(templatePlaceholders);

        var messageBuilder = new StringBuilder(emailMessage.EmailTemplate.Content);
        templatePlaceholders.ForEach(placeholder => messageBuilder.Replace(placeholder.Placeholder, placeholder.Value));

        var message = messageBuilder.ToString();
        emailMessage.Body = message;
        emailMessage.Subject = emailMessage.EmailTemplate.Subject;

        return ValueTask.FromResult(message);
    }

    private void ValidatePlaceholders(IEnumerable<TemplatePlaceholder> templatePlaceholders)
    {
        var missingPlaceholders = templatePlaceholders.Where(placeholder => !placeholder.IsValid)
            .Select(placeholder => placeholder.PlaceholderValue)
            .ToList();

        if (!missingPlaceholders.Any()) return;

        var errorMessage = new StringBuilder();
        missingPlaceholders.ForEach(placeholderValue => errorMessage.Append(placeholderValue).Append(','));

        throw new InvalidOperationException($"Variable for given placeholders is not found - {string.Join(',', missingPlaceholders)}");
    }
}