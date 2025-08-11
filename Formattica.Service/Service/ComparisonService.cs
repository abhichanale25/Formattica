using Formattica.Models.Comparison;
using Formattica.Service.IService;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Formattica.Service.Service
{
    public class ComparisonService : IComparisonService
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

        /*        public CodeComparisonResult Compare(string oldCode, string newCode)
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
                }*/


        /* public CodeComparisonResult Compare(string oldCode, string newCode)
         {
             var result = new CodeComparisonResult();

             var oldLines = oldCode.Split('\n');
             var newLines = newCode.Split('\n');

             int maxLines = Math.Max(oldLines.Length, newLines.Length);

             for(int i = 0; i < maxLines; i++)
             {
                 string oldLine = i < oldLines.Length ? oldLines[i].TrimEnd('\r') : "";
                 string newLine = i < newLines.Length ? newLines[i].TrimEnd('\r') : "";

                 var oldChars = oldLine.ToCharArray();
                 var newChars = newLine.ToCharArray();

                 var oldSegments = new List<Segment>();
                 var newSegments = new List<Segment>();

                 // LCS table
                 int[,] lcs = new int[oldChars.Length + 1, newChars.Length + 1];

                 for(int x = 0; x < oldChars.Length; x++)
                 {
                     for(int y = 0; y < newChars.Length; y++)
                     {
                         if(oldChars[x] == newChars[y])
                             lcs[x + 1, y + 1] = lcs[x, y] + 1;
                         else
                             lcs[x + 1, y + 1] = Math.Max(lcs[x + 1, y], lcs[x, y + 1]);
                     }
                 }

                 // Walk through LCS to build diff segments
                 int iOld = oldChars.Length;
                 int iNew = newChars.Length;
                 var oldSegStack = new Stack<Segment>();
                 var newSegStack = new Stack<Segment>();

                 while(iOld > 0 && iNew > 0)
                 {
                     if(oldChars[iOld - 1] == newChars[iNew - 1])
                     {
                         oldSegStack.Push(new Segment { Text = oldChars[iOld - 1].ToString(), Type = "unchanged" });
                         newSegStack.Push(new Segment { Text = newChars[iNew - 1].ToString(), Type = "unchanged" });
                         iOld--;
                         iNew--;
                     }
                     else if(lcs[iOld - 1, iNew] >= lcs[iOld, iNew - 1])
                     {
                         oldSegStack.Push(new Segment { Text = oldChars[iOld - 1].ToString(), Type = "removed" });
                         newSegStack.Push(new Segment { Text = "", Type = "empty" }); // placeholder
                         iOld--;
                     }
                     else
                     {
                         oldSegStack.Push(new Segment { Text = "", Type = "empty" }); // placeholder
                         newSegStack.Push(new Segment { Text = newChars[iNew - 1].ToString(), Type = "added" });
                         iNew--;
                     }
                 }

                 // Remaining old chars
                 while(iOld > 0)
                 {
                     oldSegStack.Push(new Segment { Text = oldChars[iOld - 1].ToString(), Type = "removed" });
                     newSegStack.Push(new Segment { Text = "", Type = "empty" });
                     iOld--;
                 }

                 // Remaining new chars
                 while(iNew > 0)
                 {
                     oldSegStack.Push(new Segment { Text = "", Type = "empty" });
                     newSegStack.Push(new Segment { Text = newChars[iNew - 1].ToString(), Type = "added" });
                     iNew--;
                 }


                 oldSegments.AddRange(oldSegStack);
                 newSegments.AddRange(newSegStack);

                 result.OldCodeLines.Add(new CodeLine { Segments = oldSegments });
                 result.NewCodeLines.Add(new CodeLine { Segments = newSegments });
             }

             return result;
         }*/

        /*        //last one

                public CodeComparisonResult Compare(string oldCode, string newCode)
                {
                    var result = new CodeComparisonResult();

                    // Normalize: multiple spaces → single space, trim each line
                    string Normalize(string input) =>
                        Regex.Replace(input, @"\s+", " ").Trim();

                    var oldNormalized = Normalize(oldCode);
                    var newNormalized = Normalize(newCode);

                    var oldLines = oldNormalized.Split('\n');
                    var newLines = newNormalized.Split('\n');

                    int maxLines = Math.Max(oldLines.Length, newLines.Length);

                    for(int i = 0; i < maxLines; i++)
                    {
                        string oldLine = i < oldLines.Length ? oldLines[i].TrimEnd('\r') : "";
                        string newLine = i < newLines.Length ? newLines[i].TrimEnd('\r') : "";

                        var oldChars = oldLine.ToCharArray();
                        var newChars = newLine.ToCharArray();

                        var oldSegments = new List<Segment>();
                        var newSegments = new List<Segment>();

                        // Build LCS table
                        int[,] lcs = new int[oldChars.Length + 1, newChars.Length + 1];

                        for(int x = 0; x < oldChars.Length; x++)
                        {
                            for(int y = 0; y < newChars.Length; y++)
                            {
                                if(oldChars[x] == newChars[y])
                                    lcs[x + 1, y + 1] = lcs[x, y] + 1;
                                else
                                    lcs[x + 1, y + 1] = Math.Max(lcs[x + 1, y], lcs[x, y + 1]);
                            }
                        }

                        int iOld = oldChars.Length;
                        int iNew = newChars.Length;
                        var oldSegStack = new Stack<Segment>();
                        var newSegStack = new Stack<Segment>();

                        while(iOld > 0 && iNew > 0)
                        {
                            // Special case: if both chars are whitespace, treat them as unchanged
                            if(char.IsWhiteSpace(oldChars[iOld - 1]) && char.IsWhiteSpace(newChars[iNew - 1]))
                            {
                                oldSegStack.Push(new Segment { Text = oldChars[iOld - 1].ToString(), Type = "unchanged" });
                                newSegStack.Push(new Segment { Text = newChars[iNew - 1].ToString(), Type = "unchanged" });
                                iOld--;
                                iNew--;
                            }
                            else if(oldChars[iOld - 1] == newChars[iNew - 1])
                            {
                                oldSegStack.Push(new Segment { Text = oldChars[iOld - 1].ToString(), Type = "unchanged" });
                                newSegStack.Push(new Segment { Text = newChars[iNew - 1].ToString(), Type = "unchanged" });
                                iOld--;
                                iNew--;
                            }
                            else if(lcs[iOld - 1, iNew] >= lcs[iOld, iNew - 1])
                            {
                                oldSegStack.Push(new Segment { Text = oldChars[iOld - 1].ToString(), Type = "removed" });
                                iOld--;
                            }
                            else
                            {
                                newSegStack.Push(new Segment { Text = newChars[iNew - 1].ToString(), Type = "added" });
                                iNew--;
                            }
                        }

                        // Any remaining characters in old/new
                        while(iOld > 0)
                        {
                            oldSegStack.Push(new Segment { Text = oldChars[iOld - 1].ToString(), Type = "removed" });
                            iOld--;
                        }
                        while(iNew > 0)
                        {
                            newSegStack.Push(new Segment { Text = newChars[iNew - 1].ToString(), Type = "added" });
                            iNew--;
                        }

                        oldSegments.AddRange(oldSegStack);
                        newSegments.AddRange(newSegStack);

                        result.OldCodeLines.Add(new CodeLine { Segments = oldSegments });
                        result.NewCodeLines.Add(new CodeLine { Segments = newSegments });
                    }

                    return result;
                }*/


        /*public CodeComparisonResult Compare(string oldCode, string newCode)
        {
            var result = new CodeComparisonResult();

            var oldLines = oldCode.Split('\n');
            var newLines = newCode.Split('\n');

            int maxLines = Math.Max(oldLines.Length, newLines.Length);

            for(int i = 0; i < maxLines; i++)
            {
                string oldLine = i < oldLines.Length ? oldLines[i].TrimEnd('\r') : "";
                string newLine = i < newLines.Length ? newLines[i].TrimEnd('\r') : "";

                // Split into words but keep spaces as separate "words"
                var oldWords = System.Text.RegularExpressions.Regex
                    .Split(oldLine, @"(\s+)")
                    .Where(w => w.Length > 0)
                    .ToArray();

                var newWords = System.Text.RegularExpressions.Regex
                    .Split(newLine, @"(\s+)")
                    .Where(w => w.Length > 0)
                    .ToArray();

                var oldSegments = new List<Segment>();
                var newSegments = new List<Segment>();

                // LCS table
                int[,] lcs = new int[oldWords.Length + 1, newWords.Length + 1];

                for(int x = 0; x < oldWords.Length; x++)
                {
                    for(int y = 0; y < newWords.Length; y++)
                    {
                        if(oldWords[x] == newWords[y])
                            lcs[x + 1, y + 1] = lcs[x, y] + 1;
                        else
                            lcs[x + 1, y + 1] = Math.Max(lcs[x + 1, y], lcs[x, y + 1]);
                    }
                }

                // Walk through LCS to build diff segments
                int iOld = oldWords.Length;
                int iNew = newWords.Length;
                var oldSegStack = new Stack<Segment>();
                var newSegStack = new Stack<Segment>();

                while(iOld > 0 && iNew > 0)
                {
                    if(oldWords[iOld - 1] == newWords[iNew - 1])
                    {
                        oldSegStack.Push(new Segment { Text = oldWords[iOld - 1], Type = "unchanged" });
                        newSegStack.Push(new Segment { Text = newWords[iNew - 1], Type = "unchanged" });
                        iOld--;
                        iNew--;
                    }
                    else if(lcs[iOld - 1, iNew] >= lcs[iOld, iNew - 1])
                    {
                        oldSegStack.Push(new Segment { Text = oldWords[iOld - 1], Type = "removed" });
                        newSegStack.Push(new Segment { Text = "", Type = "empty" });
                        iOld--;
                    }
                    else
                    {
                        oldSegStack.Push(new Segment { Text = "", Type = "empty" });
                        newSegStack.Push(new Segment { Text = newWords[iNew - 1], Type = "added" });
                        iNew--;
                    }
                }

                // Remaining old words
                while(iOld > 0)
                {
                    oldSegStack.Push(new Segment { Text = oldWords[iOld - 1], Type = "removed" });
                    newSegStack.Push(new Segment { Text = "", Type = "empty" });
                    iOld--;
                }

                // Remaining new words
                while(iNew > 0)
                {
                    oldSegStack.Push(new Segment { Text = "", Type = "empty" });
                    newSegStack.Push(new Segment { Text = newWords[iNew - 1], Type = "added" });
                    iNew--;
                }

                oldSegments.AddRange(oldSegStack);
                newSegments.AddRange(newSegStack);

                result.OldCodeLines.Add(new CodeLine { Segments = oldSegments });
                result.NewCodeLines.Add(new CodeLine { Segments = newSegments });
            }

            return result;
        }*/



        public CodeComparisonResult Compare(string oldCode, string newCode)
        {
            var result = new CodeComparisonResult();

            var oldLines = oldCode.Split('\n');
            var newLines = newCode.Split('\n');

            int maxLines = Math.Max(oldLines.Length, newLines.Length);

            for(int i = 0; i < maxLines; i++)
            {
                string oldLine = i < oldLines.Length ? oldLines[i].TrimEnd('\r') : "";
                string newLine = i < newLines.Length ? newLines[i].TrimEnd('\r') : "";

                var oldSegments = new List<Segment>();
                var newSegments = new List<Segment>();

                // Keep words + exact spaces
                var oldWords = System.Text.RegularExpressions.Regex.Split(oldLine, @"(\s+)");
                var newWords = System.Text.RegularExpressions.Regex.Split(newLine, @"(\s+)");

                int[,] lcs = new int[oldWords.Length + 1, newWords.Length + 1];

                for(int x = 0; x < oldWords.Length; x++)
                {
                    for(int y = 0; y < newWords.Length; y++)
                    {
                        if(oldWords[x] == newWords[y])
                            lcs[x + 1, y + 1] = lcs[x, y] + 1;
                        else
                            lcs[x + 1, y + 1] = Math.Max(lcs[x + 1, y], lcs[x, y + 1]);
                    }
                }

                int iOld = oldWords.Length;
                int iNew = newWords.Length;
                var oldSegStack = new Stack<Segment>();
                var newSegStack = new Stack<Segment>();

                while(iOld > 0 && iNew > 0)
                {
                    if(oldWords[iOld - 1] == newWords[iNew - 1])
                    {
                        oldSegStack.Push(new Segment { Text = oldWords[iOld - 1], Type = "unchanged" });
                        newSegStack.Push(new Segment { Text = newWords[iNew - 1], Type = "unchanged" });
                        iOld--;
                        iNew--;
                    }
                    else if(lcs[iOld - 1, iNew] >= lcs[iOld, iNew - 1])
                    {
                        oldSegStack.Push(new Segment { Text = oldWords[iOld - 1], Type = string.IsNullOrWhiteSpace(oldWords[iOld - 1]) ? "unchanged" : "removed" });
                        newSegStack.Push(new Segment { Text = "", Type = "empty" });
                        iOld--;
                    }
                    else
                    {
                        oldSegStack.Push(new Segment { Text = "", Type = "empty" });
                        newSegStack.Push(new Segment { Text = newWords[iNew - 1], Type = string.IsNullOrWhiteSpace(newWords[iNew - 1]) ? "unchanged" : "added" });
                        iNew--;
                    }
                }

                while(iOld > 0)
                {
                    oldSegStack.Push(new Segment { Text = oldWords[iOld - 1], Type = string.IsNullOrWhiteSpace(oldWords[iOld - 1]) ? "unchanged" : "removed" });
                    newSegStack.Push(new Segment { Text = "", Type = "empty" });
                    iOld--;
                }

                while(iNew > 0)
                {
                    oldSegStack.Push(new Segment { Text = "", Type = "empty" });
                    newSegStack.Push(new Segment { Text = newWords[iNew - 1], Type = string.IsNullOrWhiteSpace(newWords[iNew - 1]) ? "unchanged" : "added" });
                    iNew--;
                }

                oldSegments.AddRange(oldSegStack);
                newSegments.AddRange(newSegStack);

                result.OldCodeLines.Add(new CodeLine { Segments = oldSegments });
                result.NewCodeLines.Add(new CodeLine { Segments = newSegments });
            }

            return result;
        }


    }


}
