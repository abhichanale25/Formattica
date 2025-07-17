using Formattica.Models.Comparison;
using Formattica.Service.IService;

namespace Formattica.Service.Service
{
    public class ComparisonService:IComparisonService
    {
        public CodeComparisonResult Compare(string oldCode, string newCode)
        {
            var oldLines = oldCode.Split('\n');
            var newLines = newCode.Split('\n');

            var result = new CodeComparisonResult();

            int maxLines = Math.Max(oldLines.Length, newLines.Length);

            for(int i = 0; i < maxLines; i++)
            {
                string oldLine = i < oldLines.Length ? oldLines[i].TrimEnd('\r') : null!;
                string newLine = i < newLines.Length ? newLines[i].TrimEnd('\r') : null!;

                if(oldLine == newLine)
                {
                    result.Differences.Add(new CodeDiffLine { Line = newLine ?? "", Type = "Unchanged" });
                }
                else if(oldLine == null)
                {
                    result.Differences.Add(new CodeDiffLine { Line = newLine, Type = "Added" });
                }
                else if(newLine == null)
                {
                    result.Differences.Add(new CodeDiffLine { Line = oldLine, Type = "Removed" });
                }
                else
                {
                    result.Differences.Add(new CodeDiffLine { Line = $"- {oldLine}", Type = "Removed" });
                    result.Differences.Add(new CodeDiffLine { Line = $"+ {newLine}", Type = "Added" });
                }
            }

            return result;
        }
    }
}
