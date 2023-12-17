using System.Reflection;
using LocalIdentity.SimpleInfra.Application.Common.Notifications.Events;
using LocalIdentity.SimpleInfra.Domain.Common.Events;
using LocalIdentity.SimpleInfra.Domain.Enums;
using LocalIdentity.SimpleInfra.Domain.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LocalIdentity.SimpleInfra.Application.Common.Notifications.Mappers;

public class NotificationEventConverter : JsonConverter<NotificationEvent>
{
    public override bool CanWrite => false;

    public override bool CanRead => true;

    public override NotificationEvent? ReadJson(
        JsonReader reader,
        Type objectType,
        NotificationEvent? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer
    )
    {
        var jsonSerializer = JsonSerializer.Create();

        var typeProperty = reader.GetValueByKey(nameof(Event.EventTypeDiscriminator));

        // var test = reader.ReadAsString();

        // Console.WriteLine(test);

        // var jsonObject = JObject.Load(reader);
        // var typeProperty = jsonObject[nameof(Event.EventTypeDiscriminator)];

        if (typeProperty == null)
            throw new ArgumentNullException(nameof(typeProperty), "The type property is missing in json.");

        var enumValue = Enum.Parse<NotificationProcessingEvent>(typeProperty);

        var testb = JsonSerializer.Create();

        NotificationEvent? eventToReturn = enumValue switch
        {
            NotificationProcessingEvent.OnProcessing => testb.Deserialize<ProcessNotificationEvent>(reader),
            NotificationProcessingEvent.OnRendering => testb.Deserialize<RenderNotificationEvent>(reader),
            NotificationProcessingEvent.OnSending => testb.Deserialize<SendNotificationEvent>(reader),
            _ => throw new JsonException($"Unable to find valid configuration for provided \"type\". TypeValue: {enumValue}.")
        };

        if (eventToReturn != null)
            throw new JsonException($"Unable to find valid configuration for provided \"type\". TypeValue: {enumValue}.");

        return eventToReturn;
    }

    public override void WriteJson(JsonWriter writer, NotificationEvent? value, JsonSerializer serializer)
    {
        if (value == null) return;

        // var jsonObject = JObject.FromObject(value);
        //
        // if (jsonObject.Type != JTokenType.Object)
        // {
        //     jsonObject.WriteTo(writer);
        // }

        writer.WriteStartObject();

        // write values using reflection 
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

// public class NotificationEventConverter : JsonConverter<NotificationEvent>
// {
//     public override NotificationEvent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//     {
//         var value = reader.GetValueByKey(nameof(Event.EventTypeDiscriminator));
//
//         if (string.IsNullOrWhiteSpace(value))
//             throw new JsonException("\"type\" value could not be empty.");
//
//         if (!Enum.TryParse<NotificationProcessingEvent>(value, true, out var typeValue))
//             throw new JsonException($"Unable to parse \"type\" value. Value: {value}.");
//
//         NotificationEvent? eventToReturn = typeValue switch
//         {
//             NotificationProcessingEvent.OnProcessing => JsonSerializer.Deserialize<ProcessNotificationEvent>(ref reader, options),
//             NotificationProcessingEvent.OnRendering => JsonSerializer.Deserialize<RenderNotificationEvent>(ref reader, options),
//             NotificationProcessingEvent.OnSending => JsonSerializer.Deserialize<SendNotificationEvent>(ref reader, options),
//             _ => throw new JsonException($"Unable to find valid configuration for provided \"type\". TypeValue: {typeValue}.")
//         };
//
//         if (eventToReturn != null)
//             throw new JsonException($"Unable to find valid configuration for provided \"type\". TypeValue: {typeValue}.");
//
//         return eventToReturn;
//     }
//
//     public override void Write(Utf8JsonWriter writer, NotificationEvent value, JsonSerializerOptions options)
//     {
//         if (value == null) return;
//
//         JsonSerializer.Serialize(writer, value, value.GetType(), options);
//     }
// }