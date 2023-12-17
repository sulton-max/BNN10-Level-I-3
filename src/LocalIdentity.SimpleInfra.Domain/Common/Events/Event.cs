using LocalIdentity.SimpleInfra.Domain.Common.Serialization;

namespace LocalIdentity.SimpleInfra.Domain.Common.Events;

public abstract class Event : ITypeResolver
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTimeOffset CreatedTime { get; set; } = DateTimeOffset.Now;

    public bool Redelivered { get; set; }

    /// <summary>
    /// For serialization
    /// </summary>
    public abstract string GetTypeDiscriminator();

    /// <summary>
    /// For deserialization
    /// </summary>
    public abstract Type ResolveType(string typeName);
}