using System;
using System.Globalization;
using System.Text.Json;

namespace BilligKwhWebApp.Core.Toolbox.JsonConverters
{
    public class StringConverter : System.Text.Json.Serialization.JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                var stringValue = reader.GetInt64();
                return stringValue.ToString(CultureInfo.InvariantCulture);
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                return reader.GetString();
            }

            throw new System.Text.Json.JsonException();
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            if (writer is null)
                throw new ArgumentNullException(nameof(writer));

            writer.WriteStringValue(value);
        }
    }
}
