using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formattica.Models.Comparison
{
    public class CodeComparisonResult
    {
        public List<CodeLine> OldCodeLines { get; set; } = new();
        public List<CodeLine> NewCodeLines { get; set; } = new();
    }

    public class CodeLine
    {
        public List<Segment> Segments { get; set; } = new();
    }

    public class Segment
    {
        public string? Text { get; set; }
        public string? Type { get; set; } // "added", "removed", "unchanged"
    }


}
