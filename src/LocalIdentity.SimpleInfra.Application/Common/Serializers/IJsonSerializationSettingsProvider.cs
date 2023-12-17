using Newtonsoft.Json;

namespace LocalIdentity.SimpleInfra.Application.Common.Serializers;

public interface IJsonSerializationSettingsProvider
{
    JsonSerializerSettings Get(bool clone = false);
}