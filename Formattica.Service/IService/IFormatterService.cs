namespace Formattica.Service.IService
{
    public interface IFormatterService
    {
        (string OriginalJson, string FormattedJson) FormatJson(string? jsonInput);
    }
}
