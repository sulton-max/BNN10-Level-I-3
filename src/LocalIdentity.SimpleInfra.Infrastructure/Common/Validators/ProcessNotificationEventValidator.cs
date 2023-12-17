using FluentValidation;
using LocalIdentity.SimpleInfra.Application.Common.Notifications.Events;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Validators;

public class ProcessNotificationEventValidator : AbstractValidator<ProcessNotificationEvent>
{
    public ProcessNotificationEventValidator()
    {
        RuleFor(history => history.ReceiverUserId).NotEqual(Guid.Empty);
    }
}