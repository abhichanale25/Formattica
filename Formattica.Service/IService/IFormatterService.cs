using Formattica.Models.Formatter;

namespace Formattica.Service.IService
{
    public interface IFormatterService
    {
        FormatResult FormatContent(FormatInputModel model);

        /*(string OriginalJson, string FormattedJson) FormatJson(string? jsonInput);*/
    }
}
