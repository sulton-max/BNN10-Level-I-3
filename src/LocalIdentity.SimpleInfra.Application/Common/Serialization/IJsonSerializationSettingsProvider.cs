using Newtonsoft.Json;

namespace LocalIdentity.SimpleInfra.Application.Common.Serialization;

public interface IJsonSerializationSettingsProvider
{
    JsonSerializerSettings Get(bool clone = false);
}