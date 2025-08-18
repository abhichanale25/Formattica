namespace Formattica.Models
{
    public class CompressedFileResult
    {
        public string Base64Content { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public string DownloadFileName { get; set; } = string.Empty;
    }

}
