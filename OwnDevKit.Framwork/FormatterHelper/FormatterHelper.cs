using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SQL.Formatter;
using SQL.Formatter.Language;
using System.Xml.Linq;

namespace OwnDevKit.Framwork.FormatterHelper
{
    public static class FormatterHelper
    {
        public static string FormatJson(string content)
        {

            try
            {
                var parsed = JToken.Parse(content);
                return parsed.ToString(Formatting.Indented);
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public static string FormatXml(string content)
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

        public static string FormatSql(string content)
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
    }
}
