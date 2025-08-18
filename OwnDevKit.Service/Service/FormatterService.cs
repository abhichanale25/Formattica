using Formattica.Models.Formatter;
using Formattica.Service.IService;
using OwnDevKit.Framwork.FormatterHelper;

namespace Formattica.Service.Service
{
    public class FormatterService:IFormatterService
    {

        public FormatResult FormatContent(FormatInputModel formatInputModel)
        {
            var original = formatInputModel.Content;
            var formatType = formatInputModel.FormatType?.ToUpper();

            string formatted = formatType switch
            {
                "JSON" => FormatterHelper.FormatJson(original!),
                "XML" => FormatterHelper.FormatXml(original!),
                "SQL" => FormatterHelper.FormatSql(original!),
                _ => "Unsupported format type. Use JSON, XML, or SQL."
            };

            return new FormatResult
            {
                Original = original!,
                Formatted = formatted
            };
        }

    }
}
