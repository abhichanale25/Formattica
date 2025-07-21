using Formattica.Models.Formatter;

namespace Formattica.Service.IService
{
    public interface IFormatterService
    {
        FormatResult FormatContent(string? Content, string? FormatType);

        /*(string OriginalJson, string FormattedJson) FormatJson(string? jsonInput);*/
    }
}
