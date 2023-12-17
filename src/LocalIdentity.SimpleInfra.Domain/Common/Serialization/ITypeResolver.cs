namespace LocalIdentity.SimpleInfra.Domain.Common.Serialization;

public interface ITypeResolver
{
    string GetTypeDiscriminatorKey() => "TypeDiscriminator";

    /// <summary>
    /// Returns the type discriminator for the implementing type.
    /// </summary>
    string GetTypeDiscriminator();

    /// <summary>
    /// Returns the type for the given discriminator.
    /// </summary>
    Type ResolveType(string typeName);
}