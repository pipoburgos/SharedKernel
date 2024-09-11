using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharedKernel.Infrastructure.NetJson;

/// <summary> . </summary>
public class DateTimeConverter : JsonConverter<DateTime>
{
    /// <summary> . </summary>
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var x = reader.GetString();
        if (DateTime.TryParseExact(x ?? string.Empty, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal, out var result2))
            return result2.ToUniversalTime();

        if (DateTime.TryParseExact(x ?? string.Empty, "yyyy-MM-ddTHH:mm:ss.FFFZ", CultureInfo.InvariantCulture,
                DateTimeStyles.AdjustToUniversal, out var result1))
            return result1;

        return default(DateTime);
    }

    /// <summary> . </summary>
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.ToUniversalTime());
}
