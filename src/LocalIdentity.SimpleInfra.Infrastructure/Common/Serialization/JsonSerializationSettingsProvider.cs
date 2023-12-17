using Force.DeepCloner;
using LocalIdentity.SimpleInfra.Application.Common.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LocalIdentity.SimpleInfra.Infrastructure.Common.Serialization;

public class JsonSerializationSettingsProvider : IJsonSerializationSettingsProvider
{
    private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
    {
        Formatting = Formatting.Indented,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        NullValueHandling = NullValueHandling.Ignore
    };

    public JsonSerializerSettings Get(bool clone = false) => clone ? _jsonSerializerSettings.DeepClone() : _jsonSerializerSettings;
}