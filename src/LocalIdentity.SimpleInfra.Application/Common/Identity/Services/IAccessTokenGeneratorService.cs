using LocalIdentity.SimpleInfra.Domain.Entities;

namespace LocalIdentity.SimpleInfra.Application.Common.Identity.Services;

public interface IAccessTokenGeneratorService
{
    AccessToken GetToken(User user);

    Guid GetTokenId(string accessToken);
}