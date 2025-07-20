using Formattica.Service.IService;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Tiff;
using SixLabors.ImageSharp.Formats.Webp;
using SQL.Formatter.Core;
using System.Text;
using System.Text.RegularExpressions;

namespace Formattica.Service.Service
{
    public class ConversionService: IConversionService
    {

        public async Task<(byte[]? ConvertedBytes, string? ContentType, string? FileExtension)> ConvertImage(IFormFile file, string targetFormat)
        {
            if(file == null || file.Length == 0 || string.IsNullOrEmpty(targetFormat))
                return (null, null, null);

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            using var image = await Image.LoadAsync(memoryStream);
            using var output = new MemoryStream();

            string contentType;
            string fileExtension;

            switch(targetFormat.ToLower())
            {
                case "jpeg":
                case "jpg":
                    await image.SaveAsJpegAsync(output, new JpegEncoder { Quality = 85 });
                    contentType = "image/jpeg";
                    fileExtension = "jpg";
                    break;

                case "png":
                    await image.SaveAsPngAsync(output, new PngEncoder());
                    contentType = "image/png";
                    fileExtension = "png";
                    break;

                case "gif":
                    await image.SaveAsGifAsync(output, new GifEncoder());
                    contentType = "image/gif";
                    fileExtension = "gif";
                    break;

                case "bmp":
                    await image.SaveAsBmpAsync(output, new BmpEncoder());
                    contentType = "image/bmp";
                    fileExtension = "bmp";
                    break;

                case "tiff":
                    await image.SaveAsTiffAsync(output, new TiffEncoder());
                    contentType = "image/tiff";
                    fileExtension = "tiff";
                    break;

                case "webp":
                    await image.SaveAsWebpAsync(output, new WebpEncoder());
                    contentType = "image/webp";
                    fileExtension = "webp";
                    break;

                default:
                    return (null, null, null);
            }

            return (output.ToArray(), contentType, fileExtension);
        }
        public Task<string> GenerateClass(string databaseType, string language, string className, string definition)
        {
            var fields = databaseType.ToLower() switch
            {
                "relational" or "columnfamily" => ParseSqlCreateTable(definition),
                "document" or "keyvalue" => ParseJsonDefinition(definition),
                _ => throw new Exception("Unsupported database type.")
            };

            var result = language.ToLower() switch
            {
                "c#" => GenerateCSharpClass(className, fields),
                "java" => GenerateJavaClass(className, fields),
                "python" => GeneratePythonClass(className, fields),
                "go" => GenerateGoStruct(className, fields),
                _ => "// Unsupported target language."
            };

            return Task.FromResult(result);
        }

        private static Dictionary<string, string> ParseSqlCreateTable(string sql)
        {
            var fields = new Dictionary<string, string>();
            var match = Regex.Match(sql, @"\((.*?)\)", RegexOptions.Singleline);
            if (match.Success)
            {
                var lines = match.Groups[1].Value.Split(new[] { ',', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    var parts = line.Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 2)
                    {
                        var name = parts[0].Trim('`', '"');
                        var type = parts[1].ToLower();
                        fields[name] = type;
                    }
                }
            }
            return fields;
        }

        private static Dictionary<string, string> ParseJsonDefinition(string json)
        {
            var fields = new Dictionary<string, string>();
            var jObject = JObject.Parse(json);
            foreach (var prop in jObject.Properties())
            {
                string type = prop.Value.Type == JTokenType.Array
                    ? $"{prop.Value.FirstOrDefault()?.ToString()}[]"
                    : prop.Value.ToString();
                fields[prop.Name] = type.ToLower();
            }
            return fields;
        }

        private static string GenerateCSharpClass(string className, Dictionary<string, string> fields)
        {
            var sb = new StringBuilder();
            sb.AppendLine("public class " + className);
            sb.AppendLine("{");
            foreach (var field in fields)
            {
                sb.AppendLine($"    public {MapToCSharpType(field.Value)} {field.Key} {{ get; set; }}");
            }
            sb.AppendLine("}");
            return sb.ToString();
        }

        private static string GenerateJavaClass(string className, Dictionary<string, string> fields)
        {
            var sb = new StringBuilder();
            sb.AppendLine("public class " + className + " {");
            foreach (var field in fields)
            {
                sb.AppendLine($"    private {MapToJavaType(field.Value)} {field.Key};");
            }
            sb.AppendLine("}");
            return sb.ToString();
        }

        private static string GeneratePythonClass(string className, Dictionary<string, string> fields)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"class {className}:");
            sb.AppendLine("    def __init__(self):");
            foreach (var field in fields)
            {
                sb.AppendLine($"        self.{field.Key} = {GetPythonDefaultValue(MapToPythonType(field.Value))}");
            }
            return sb.ToString();
        }

        private static string GenerateGoStruct(string className, Dictionary<string, string> fields)
        {
            var sb = new StringBuilder();
            sb.AppendLine("type " + className + " struct {");
            foreach (var field in fields)
            {
                sb.AppendLine($"    {ToPascalCase(field.Key)} {MapToGoType(field.Value)} `json:\"{field.Key}\"`");
            }
            sb.AppendLine("}");
            return sb.ToString();
        }

        // Type Mapping Helpers
        private static string MapToCSharpType(string type) => type switch
        {
            var t when t.Contains("int") => "int",
            var t when t.Contains("decimal") || t.Contains("float") => "decimal",
            var t when t.Contains("bool") => "bool",
            var t when t.Contains("date") => "DateTime",
            var t when t.Contains("[]") => "List<string>",
            _ => "string"
        };

        private static string MapToJavaType(string type) => type switch
        {
            var t when t.Contains("int") => "int",
            var t when t.Contains("decimal") || t.Contains("float") => "double",
            var t when t.Contains("bool") => "boolean",
            var t when t.Contains("date") => "LocalDate",
            var t when t.Contains("[]") => "List<String>",
            _ => "String"
        };

        private static string MapToPythonType(string type) => type switch
        {
            var t when t.Contains("int") => "int",
            var t when t.Contains("decimal") || t.Contains("float") => "float",
            var t when t.Contains("bool") => "bool",
            var t when t.Contains("date") => "datetime",
            var t when t.Contains("[]") => "List[str]",
            _ => "str"
        };

        private static string GetPythonDefaultValue(string type) => type switch
        {
            "int" => "0",
            "float" => "0.0",
            "bool" => "False",
            "datetime" => "None",
            "List[str]" => "[]",
            _ => "''"
        };

        private static string MapToGoType(string type) => type switch
        {
            var t when t.Contains("int") => "int",
            var t when t.Contains("decimal") || t.Contains("float") => "float64",
            var t when t.Contains("bool") => "bool",
            var t when t.Contains("date") => "time.Time",
            var t when t.Contains("[]") => "[]string",
            _ => "string"
        };

        private static string ToPascalCase(string str) =>
            string.Join("", str.Split('_', '-', ' ')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1)));
    }
}
