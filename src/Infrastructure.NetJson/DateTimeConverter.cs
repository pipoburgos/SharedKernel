using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharedKernel.Infrastructure.NetJson;

/// <summary>  </summary>
public class DateTimeConverter : JsonConverter<DateTime>
{
    /// <summary>  </summary>
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        DateTime.ParseExact(reader.GetString() ?? string.Empty, "yyyy-MM-ddTHH:mm:ss.FFFZ", CultureInfo.InvariantCulture).ToUniversalTime();

    /// <summary>  </summary>
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.ToUniversalTime());
}
