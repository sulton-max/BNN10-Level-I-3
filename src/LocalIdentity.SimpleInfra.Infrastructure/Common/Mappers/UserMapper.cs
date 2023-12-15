using AutoMapper;
using LocalIdentity.SimpleInfra.Application.Common.Identity.Models;
using LocalIdentity.SimpleInfra.Application.Common.Identity.Services;
using LocalIdentity.SimpleInfra.Domain.Entities;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Mappers;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<SignUpDetails, User>();
    }
}