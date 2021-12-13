using System.Text.Json;

namespace DbControlCore.Helpers
{
    public static class JsonHelper
    {
        public static string SerializeObject<T>(T item, bool writeIndented = true)
        {
            return JsonSerializer.Serialize(item, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = writeIndented
            });
        }

        public static T DerializeObject<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }
}
