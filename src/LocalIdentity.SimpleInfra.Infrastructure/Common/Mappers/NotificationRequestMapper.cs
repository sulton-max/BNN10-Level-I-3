using AutoMapper;
using LocalIdentity.SimpleInfra.Application.Common.Notfications.Models;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Mappers;

public class NotificationRequestMapper : Profile
{
    public NotificationRequestMapper()
    {
        CreateMap<NotificationRequest, EmailNotificationRequest>();
    }
}