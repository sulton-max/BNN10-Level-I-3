using AutoMapper;
using LocalIdentity.SimpleInfra.Application.Common.Notfications.Models;
using LocalIdentity.SimpleInfra.Domain.Entities;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Mappers;

public class NotificationHistoryMapper : Profile
{
    public NotificationHistoryMapper()
    {
        CreateMap<EmailMessage, EmailHistory>()
            .ForMember(dest => dest.TemplateId, opt => opt.MapFrom(src => src.Template.Id))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Body));
    }
}