using System.Reflection;
using LocalIdentity.SimpleInfra.Domain.Common.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LocalIdentity.SimpleInfra.Domain.Serializers;

public class JsonTypeConverter<T> : JsonConverter<T> where T : ITypeResolver
{
    public override T? ReadJson(JsonReader reader, Type objectType, T? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null) return default;

        var jObject = JObject.Load(reader);

        // Create base type and get type discriminator
        var baseType = Activator.CreateInstance<T>();
        var typeDiscriminator = jObject.Value<string>(baseType.GetTypeDiscriminatorKey());
        if (typeDiscriminator == null) throw new ArgumentNullException(nameof(typeDiscriminator), "The type property is missing in json.");
        var targetType = baseType.ResolveType(typeDiscriminator);

        // Check if target type can be assigned to base type
        if (!targetType.IsAssignableTo(typeof(T)))
            throw new JsonException(
                $"Invalid type {targetType.Name} to deserialize as {typeof(T).Name}, type discriminator value : {typeDiscriminator}."
            );

        // Create target instance and populate values
        var target = (T)Activator.CreateInstance(targetType)!;
        serializer.Populate(jObject.CreateReader(), target!);

        return target;
    }

    public override void WriteJson(JsonWriter writer, T? value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        // Write type discriminator
        var typeValue = typeof(T);
        if (typeValue.IsAbstract || typeValue.IsInterface)
        {
            // Write type discriminator
            writer.WritePropertyName(value.GetTypeDiscriminatorKey());
            writer.WriteValue(value.GetTypeDiscriminator());
        }

        writer.WritePropertyName(value!.GetTypeDiscriminatorKey());
        writer.WriteValue(value!.GetTypeDiscriminator());

        // Write instance values 
        value.GetType()
            .GetProperties()
            .Where(property => property.PropertyType.GetCustomAttribute<JsonIgnoreAttribute>() == null && property.CanRead)
            .ToList()
            .ForEach(
                property =>
                {
                    writer.WritePropertyName(property.Name);
                    serializer.Serialize(writer, property.GetValue(value));
                }
            );

        writer.WriteEndObject();
    }
}