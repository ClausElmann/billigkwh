using BilligKwhWebApp.Core.Toolbox.JsonConverters;
using System.Text.Json;

namespace BilligKwhWebApp.Core.Toolbox
{
    public static class JsonHelper
    {
        public static JsonSerializerOptions JsonSerializerOptions { get; } = new()
        {
            WriteIndented = false,
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            ReadCommentHandling = JsonCommentHandling.Skip,
            Converters = {
                    new System.Text.Json.Serialization.JsonStringEnumConverter(),
                    new DateTimeConverter(),
                    new StringConverter(),
                    new IntConverter(),
                    new DoubleConverter(),
                    new LongConverter(),
                    new JsonTimeSpanConverter()
                }
        };
    }
}
