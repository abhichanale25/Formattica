namespace Formattica.Models.Models
{
    public class CodeCompare
    {
        public string? Code1 { get; set; }
        public string? Code2 { get; set; }
        public List<CodeDiffResult> Differences { get; set; } = new();
    }

    public class CodeDiffResult
    {
        public int LineNumber { get; set; }
        public string? LineType { get; set; } // "Added", "Removed", "Unchanged"
        public string? Code { get; set; }
    }


}

