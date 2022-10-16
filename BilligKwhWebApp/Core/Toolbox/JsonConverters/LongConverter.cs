using System;
using System.Globalization;
using System.Text.Json;

namespace BilligKwhWebApp.Core.Toolbox.JsonConverters
{
    public class LongConverter : System.Text.Json.Serialization.JsonConverter<long>
    {
        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var stringValue = reader.GetString();
                if (string.IsNullOrEmpty(stringValue)) return 0;
                return long.Parse(stringValue, CultureInfo.InvariantCulture);
            }

            return reader.GetInt64();
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            if (writer is null)
                throw new ArgumentNullException(nameof(writer));
            writer.WriteNumberValue(value);
        }
    }
}
