using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using Formattica.Models.Comparison;
using Formattica.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace Formattica.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComparisonController : ControllerBase
    {
        private readonly IComparisonService _comparisonService;

        public ComparisonController(IComparisonService comparisonService)
        {
            _comparisonService = comparisonService;
        }

        [HttpPost("compare1")]
        public IActionResult CompareCode([FromBody] CodeComparisonRequest request)
        {
            var result = _comparisonService.Compare(request.OldCode!, request.NewCode!);
            return Ok(result);
        }


        [HttpPost("compare")]
        public IActionResult Compare([FromBody] CodeComparisonRequest request)
        {
            if(string.IsNullOrWhiteSpace(request.OldCode) || string.IsNullOrWhiteSpace(request.NewCode))
                return BadRequest("Input strings cannot be empty.");

            var result = CompareLines(request.OldCode, request.NewCode);
            return Ok(result);
        }

        private List<object> CompareLines(string oldCode, string newCode)
        {
            var oldLines = oldCode.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            var newLines = newCode.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            int m = oldLines.Length;
            int n = newLines.Length;

            var trimmedOld = oldLines.Select(l => l.Trim()).ToArray();
            var trimmedNew = newLines.Select(l => l.Trim()).ToArray();

            int[,] lcs = new int[m + 1, n + 1];
            for(int i = m - 1; i >= 0; i--)
            {
                for(int j = n - 1; j >= 0; j--)
                {
                    if(trimmedOld[i] == trimmedNew[j])
                        lcs[i, j] = lcs[i + 1, j + 1] + 1;
                    else
                        lcs[i, j] = Math.Max(lcs[i + 1, j], lcs[i, j + 1]);
                }
            }

            var result = new List<object>();
            int a = 0, b = 0;
            while(a < m && b < n)
            {
                if(trimmedOld[a] == trimmedNew[b])
                {
                    result.Add(new { type = "Unchanged", text = newLines[b] });
                    a++; b++;
                }
                else if(lcs[a + 1, b] >= lcs[a, b + 1])
                {
                    // Try inline diff if likely similar
                    if(b < n)
                    {
                        var wordDiff = CompareWords(oldLines[a], newLines[b]);
                        result.AddRange(wordDiff);
                        a++; b++;
                    }
                    else
                    {
                        result.Add(new { type = "Deleted", text = oldLines[a++] });
                    }
                }
                else
                {
                    result.Add(new { type = "Inserted", text = newLines[b++] });
                }
            }

            while(a < m)
                result.Add(new { type = "Deleted", text = oldLines[a++] });

            while(b < n)
                result.Add(new { type = "Inserted", text = newLines[b++] });

            return result;
        }

        private List<object> CompareWords(string oldLine, string newLine)
        {
            var oldWords = oldLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var newWords = newLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            int m = oldWords.Length;
            int n = newWords.Length;
            int[,] lcs = new int[m + 1, n + 1];

            for(int i = m - 1; i >= 0; i--)
            {
                for(int j = n - 1; j >= 0; j--)
                {
                    if(oldWords[i] == newWords[j])
                        lcs[i, j] = lcs[i + 1, j + 1] + 1;
                    else
                        lcs[i, j] = Math.Max(lcs[i + 1, j], lcs[i, j + 1]);
                }
            }

            var result = new List<object>();
            int a = 0, b = 0;

            while(a < m && b < n)
            {
                if(oldWords[a] == newWords[b])
                {
                    result.Add(new { type = "Unchanged", text = oldWords[a] });
                    a++; b++;
                }
                else if(lcs[a + 1, b] >= lcs[a, b + 1])
                {
                    result.Add(new { type = "Deleted", text = oldWords[a++] });
                }
                else
                {
                    result.Add(new { type = "Inserted", text = newWords[b++] });
                }
            }

            while(a < m)
                result.Add(new { type = "Deleted", text = oldWords[a++] });

            while(b < n)
                result.Add(new { type = "Inserted", text = newWords[b++] });

            return result;
        }


        #region Old Code
        /*private List<object> CompareLines(string oldCode, string newCode)
{
    var oldLines = oldCode.Split('\n');
    var newLines = newCode.Split('\n');

    int m = oldLines.Length;
    int n = newLines.Length;

    // Trim and normalize line endings
    for(int x = 0; x < m; x++) oldLines[x] = oldLines[x].TrimEnd('\r');
    for(int x = 0; x < n; x++) newLines[x] = newLines[x].TrimEnd('\r');

    // Step 1: LCS Table
    int[,] lcs = new int[m + 1, n + 1];
    for(int i = m - 1; i >= 0; i--)
    {
        for(int j = n - 1; j >= 0; j--)
        {
            if(oldLines[i] == newLines[j])
                lcs[i, j] = lcs[i + 1, j + 1] + 1;
            else
                lcs[i, j] = Math.Max(lcs[i + 1, j], lcs[i, j + 1]);
        }
    }

    // Step 2: Backtrack and compare
    var result = new List<object>();
    int a = 0, b = 0;

    while(a < m && b < n)
    {
        if(oldLines[a] == newLines[b])
        {
            result.Add(new { type = "Unchanged", text = oldLines[a] });
            a++; b++;
        }
        else if(lcs[a + 1, b] >= lcs[a, b + 1])
        {
            result.Add(new { type = "Deleted", text = oldLines[a] });
            a++;
        }
        else
        {
            result.Add(new { type = "Inserted", text = newLines[b] });
            b++;
        }
    }

    // Remaining deleted
    while(a < m)
    {
        result.Add(new { type = "Deleted", text = oldLines[a] });
        a++;
    }

    // Remaining inserted
    while(b < n)
    {
        result.Add(new { type = "Inserted", text = newLines[b] });
        b++;
    }

    return result;
}*/

        //[HttpPost("compare")]
        //public IActionResult Compare([FromBody] CodeComparisonRequest request)
        //{
        //    var diffBuilder = new InlineDiffBuilder(new Differ());
        //    var diff = diffBuilder.BuildDiffModel(request.OldCode, request.NewCode);

        //    var result = diff.Lines.Select(line => new
        //    {
        //        Type = line.Type.ToString(), // Inserted, Deleted, Unchanged, Imaginary
        //        Text = line.Text
        //    });

        //    return Ok(result);
        //}

        /*        [HttpPost("compare")]
                public IActionResult Compare([FromBody] CodeComparisonRequest request)
                {
                    if(string.IsNullOrWhiteSpace(request.OldCode) || string.IsNullOrWhiteSpace(request.NewCode))
                        return BadRequest("Input strings cannot be empty.");

                    var result = CompareStringsInline(request.OldCode, request.NewCode);
                    return Ok(result);
                }

                private List<object> CompareStringsInline(string oldStr, string newStr)
                {
                    var result = new List<object>();

                    int oldLen = oldStr.Length;
                    int newLen = newStr.Length;
                    int i = 0, j = 0;

                    while(i < oldLen && j < newLen)
                    {
                        if(oldStr[i] == newStr[j])
                        {
                            string common = oldStr[i].ToString();
                            result.Add(new { type = "Unchanged", text = common });
                            i++;
                            j++;
                        }
                        else
                        {
                            result.Add(new { type = "Deleted", text = oldStr[i].ToString() });
                            result.Add(new { type = "Inserted", text = newStr[j].ToString() });
                            i++;
                            j++;
                        }
                    }

                    // Remaining deleted characters
                    while(i < oldLen)
                    {
                        result.Add(new { type = "Deleted", text = oldStr[i].ToString() });
                        i++;
                    }

                    // Remaining inserted characters
                    while(j < newLen)
                    {
                        result.Add(new { type = "Inserted", text = newStr[j].ToString() });
                        j++;
                    }

                    return result;
                }*/ 
        #endregion


    }
}
