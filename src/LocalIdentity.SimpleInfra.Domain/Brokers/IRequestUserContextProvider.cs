namespace LocalIdentity.SimpleInfra.Domain.Brokers;

public interface IRequestUserContextProvider
{
    Guid GetUserIdAsync(CancellationToken cancellationToken = default);
}