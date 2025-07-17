using Formattica.Models.Formatter;
using Formattica.Service.IService;
using SQL.Formatter;
using SQL.Formatter.Language;
using System.Text.Json;
using System.Xml.Linq;

namespace Formattica.Service.Service
{
    public class FormatterService:IFormatterService
    {

        public FormatResult FormatContent(FormatInputModel model)
        {
            var original = model.Content;
            var formatType = model.FormatType?.ToUpper();

            string formatted = formatType switch
            {
                "JSON" => FormatJson(original!),
                "XML" => FormatXml(original!),
                "SQL" => FormatSql(original!),
                _ => "Unsupported format type. Use JSON, XML, or SQL."
            };

            return new FormatResult
            {
                Original = original!,
                Formatted = formatted
            };
        }

        private string FormatJson(string content)
        {
            try
            {
                var jsonElement = JsonSerializer.Deserialize<JsonElement>(content);
                return JsonSerializer.Serialize(jsonElement, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
            }
            catch(Exception ex)
            {
                return $"Invalid JSON: {ex.Message}";
            }
        }

        private string FormatXml(string content)
        {
            try
            {
                var doc = XDocument.Parse(content);
                return doc.Declaration + Environment.NewLine + doc.ToString();
            }
            catch(Exception ex)
            {
                return $"Invalid XML: {ex.Message}";
            }
        }

        private string FormatSql(string content)
        {
            try
            {
                return SqlFormatter.Of(Dialect.TSql).Format(content);
            }
            catch(Exception ex)
            {
                return $"SQL formatting error: {ex.Message}";
            }
        }


        /*public (string OriginalJson, string FormattedJson) FormatJson(string? jsonInput)
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
        }*/
    }
}
