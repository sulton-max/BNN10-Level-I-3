using FluentValidation;
using LocalIdentity.SimpleInfra.Application.Common.Notfications.Models;
using LocalIdentity.SimpleInfra.Domain.Enums;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Validators;

public class EmailMessageValidator : AbstractValidator<EmailMessage>
{
    public EmailMessageValidator()
    {
        RuleSet(NotificationEvent.OnRendering.ToString(),
            () =>
            {
                RuleFor(history => history.Template).NotNull();
                RuleFor(history => history.Variables).NotNull();
                RuleFor(history => history.Template.Content).NotNull().NotEmpty();
            });

        RuleSet(NotificationEvent.OnSending.ToString(),
            () =>
            {
                RuleFor(history => history.SendEmailAddress).NotNull().NotEmpty();
                RuleFor(history => history.ReceiverEmailAddress).NotNull().NotEmpty();
                RuleFor(history => history.Subject).NotNull().NotEmpty();
                RuleFor(history => history.Body).NotNull().NotEmpty();
            });
    }
}