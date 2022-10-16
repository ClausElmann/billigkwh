using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BilligKwhWebApp.Core.Toolbox.JsonConverters
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString(), CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            if (writer is null)
                throw new ArgumentNullException(nameof(writer));
            if (options is null)
                throw new ArgumentNullException(nameof(options));

            var utcDate = value.Kind == DateTimeKind.Local ? value.ToUniversalTime() : DateTime.SpecifyKind(value, DateTimeKind.Utc);

            writer.WriteStringValue(utcDate.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ", CultureInfo.InvariantCulture));
        }
    }
}
