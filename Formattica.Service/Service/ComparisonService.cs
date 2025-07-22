using Formattica.Models.Comparison;
using Formattica.Service.IService;

namespace Formattica.Service.Service
{
    public class ComparisonService:IComparisonService
    {
        //public CodeComparisonResult Compare(string oldCode, string newCode)
        //{
        //    var oldLines = oldCode.Split('\n');
        //    var newLines = newCode.Split('\n');

        //    var result = new CodeComparisonResult();

        //    int maxLines = Math.Max(oldLines.Length, newLines.Length);

        //    for(int i = 0; i < maxLines; i++)
        //    {
        //        string oldLine = i < oldLines.Length ? oldLines[i].TrimEnd('\r') : null!;
        //        string newLine = i < newLines.Length ? newLines[i].TrimEnd('\r') : null!;

        //        if(oldLine == newLine)
        //        {
        //            result.Differences.Add(new CodeDiffLine { Line = newLine ?? "", Type = "Unchanged" });
        //        }
        //        else if(oldLine == null)
        //        {
        //            result.Differences.Add(new CodeDiffLine { Line = newLine, Type = "Added" });
        //        }
        //        else if(newLine == null)
        //        {
        //            result.Differences.Add(new CodeDiffLine { Line = oldLine, Type = "Removed" });
        //        }
        //        else
        //        {
        //            result.Differences.Add(new CodeDiffLine { Line = $"- {oldLine}", Type = "Removed" });
        //            result.Differences.Add(new CodeDiffLine { Line = $"+ {newLine}", Type = "Added" });
        //        }
        //    }

        //    return result;
        //}

        public CodeComparisonResult Compare(string oldCode, string newCode)
        {
            var result = new CodeComparisonResult();

            var oldLines = oldCode.Split('\n');
            var newLines = newCode.Split('\n');

            int maxLines = Math.Max(oldLines.Length, newLines.Length);

            for (int i = 0; i < maxLines; i++)
            {
                string oldLine = i < oldLines.Length ? oldLines[i].TrimEnd('\r') : "";
                string newLine = i < newLines.Length ? newLines[i].TrimEnd('\r') : "";

                var oldWords = oldLine.Split(' ', StringSplitOptions.None);
                var newWords = newLine.Split(' ', StringSplitOptions.None);

                var oldSegments = new List<Segment>();
                var newSegments = new List<Segment>();

                int maxWords = Math.Max(oldWords.Length, newWords.Length);

                for (int j = 0; j < maxWords; j++)
                {
                    string? oldWord = j < oldWords.Length ? oldWords[j] : null;
                    string? newWord = j < newWords.Length ? newWords[j] : null;

                    if (oldWord == newWord)
                    {
                        if (oldWord != null)
                        {
                            oldSegments.Add(new Segment { Text = oldWord + " ", Type = "unchanged" });
                            newSegments.Add(new Segment { Text = newWord + " ", Type = "unchanged" });
                        }
                    }
                    else
                    {
                        if (oldWord != null)
                            oldSegments.Add(new Segment { Text = oldWord + " ", Type = "removed" });

                        if (newWord != null)
                            newSegments.Add(new Segment { Text = newWord + " ", Type = "added" });
                    }
                }

                result.OldCodeLines.Add(new CodeLine { Segments = oldSegments });
                result.NewCodeLines.Add(new CodeLine { Segments = newSegments });
            }

            return result;
        }



    }


}
