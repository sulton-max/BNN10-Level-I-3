using LocalIdentity.SimpleInfra.Domain.Entities;

namespace LocalIdentity.SimpleInfra.Application.Common.Identity.Services;

public interface IAccountAggregatorService
{
    ValueTask<bool> CreateUserAsync(User user, CancellationToken cancellationToken = default);
}