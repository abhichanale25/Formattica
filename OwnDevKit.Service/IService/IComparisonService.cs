using Formattica.Models.Comparison;

namespace Formattica.Service.IService
{
    public interface IComparisonService
    {
        CodeComparisonResult Compare(string oldCode, string newCode);
    }
}
