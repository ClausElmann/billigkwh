using System;
using System.Globalization;
using System.Text.Json;

namespace BilligKwhWebApp.Core.Toolbox.JsonConverters
{
    public class IntConverter : System.Text.Json.Serialization.JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var stringValue = reader.GetString();
                return int.Parse(stringValue, CultureInfo.InvariantCulture);
            }

            return reader.GetInt32();
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            if (writer is null)
                throw new ArgumentNullException(nameof(writer));
            writer.WriteNumberValue(value);
        }
    }
}
