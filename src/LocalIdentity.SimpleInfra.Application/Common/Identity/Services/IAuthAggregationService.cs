using LocalIdentity.SimpleInfra.Application.Common.Identity.Models;

namespace LocalIdentity.SimpleInfra.Application.Common.Identity.Services;

public interface IAuthAggregationService
{
    ValueTask<bool> SignUpAsync(SignUpDetails signUpDetails, CancellationToken cancellationToken = default);
}