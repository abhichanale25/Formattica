using Formattica.Service.IService;
using System.Text.Json;

namespace Formattica.Service.Service
{
    public class FormatterService:IFormatterService
    {
        public (string OriginalJson, string FormattedJson) FormatJson(string? jsonInput)
        {
            if(string.IsNullOrWhiteSpace(jsonInput))
                return (jsonInput ?? string.Empty, "Invalid JSON format!");

            try
            {
                using var jsonDoc = JsonDocument.Parse(jsonInput);
                var formatted = JsonSerializer.Serialize(jsonDoc, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                return (jsonInput, formatted);
            }
            catch(JsonException)
            {
                return (jsonInput, "Invalid JSON format!");
            }
        }
    }
}
