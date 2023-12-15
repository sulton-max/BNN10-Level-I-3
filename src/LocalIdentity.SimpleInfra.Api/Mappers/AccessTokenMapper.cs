using AutoMapper;
using LocalIdentity.SimpleInfra.Api.Models.Dtos;
using LocalIdentity.SimpleInfra.Application.Common.Identity.Models;
using LocalIdentity.SimpleInfra.Domain.Entities;

namespace LocalIdentity.SimpleInfra.Api.Mappers;

public class AccessTokenMapper : Profile
{
    public AccessTokenMapper()
    {
        CreateMap<AccessToken, AccessTokenDto>().ReverseMap();
    }
}