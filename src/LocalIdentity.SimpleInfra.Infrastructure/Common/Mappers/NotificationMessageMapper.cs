using AutoMapper;
using LocalIdentity.SimpleInfra.Application.Common.Notfications.Models;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Mappers;

public class NotificationMessageMapper : Profile
{
    public NotificationMessageMapper()
    {
        CreateMap<EmailNotificationRequest, EmailMessage>();
    }
}