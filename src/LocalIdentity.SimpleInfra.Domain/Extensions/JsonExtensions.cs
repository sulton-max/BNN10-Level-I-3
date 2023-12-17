using System.Reflection;
using Newtonsoft.Json;
using JsonException = System.Text.Json.JsonException;

namespace LocalIdentity.SimpleInfra.Domain.Extensions;

public static class JsonExtensions
{
    public static IList<JsonConverter> GetJsonConverters(this Type type)
    {
        var attribute = type.GetCustomAttribute<JsonConverterAttribute>();
        return attribute is null ? [] : [(JsonConverter)Activator.CreateInstance(attribute.ConverterType)!];
    }

    // public static ReadOnlySpan<byte> GetValue(this ref Utf8JsonReader reader, ReadOnlySpan<byte> keyValue)
    // {
    //     ReadOnlySpan<byte> value = null;
    //
    //     while (reader.Read())
    //     {
    //         if (reader.TokenType == JsonTokenType.EndObject) break;
    //         if (reader.TokenType == JsonTokenType.PropertyName && reader.ValueTextEquals(keyValue))
    //         {
    //             reader.Skip();
    //             value = reader.ValueSpan;
    //
    //             break;
    //         }
    //
    //         reader.Skip();
    //     }
    //
    //     return value;
    // }
    //
    public static string GetValueByKey(this JsonReader reader, string keyToMatch)
    {
        // var keyValue = Encoding.UTF8.GetBytes(keyToMatch).AsSpan();

        while (reader.Read())
        {
            if (reader.TokenType != JsonToken.PropertyName ||
                !reader.Value!.ToString()!.Equals(keyToMatch, StringComparison.OrdinalIgnoreCase)) continue;

            reader.Read();
            return (string)reader.Value!;
        }

        // while (reader.Read())
        // {
        //     if (reader.TokenType != JsonTokenType.PropertyName || reader.ValueSpan != keyValue) continue;
        //     reader.Read(); // Advance to next token which should be the value
        //     if (reader.TokenType == JsonTokenType.String) return reader.ValueSpan.ToString();
        //
        //     throw new JsonException($"Expected a string value for property {keyToMatch.ToString()}, but found {reader.TokenType}.");
        // }

        throw new JsonException($"Could not find property {keyToMatch} in JSON.");
    }
}