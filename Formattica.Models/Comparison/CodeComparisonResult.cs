using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formattica.Models.Comparison
{
    public class CodeComparisonResult1
    {
        public List<CodeDiffLine> Differences { get; set; } = new();
    }

    public class CodeDiffLine
    {
        public string? Line { get; set; }
        public string? Type { get; set; } // "Added", "Removed", "Unchanged"
    }

}
