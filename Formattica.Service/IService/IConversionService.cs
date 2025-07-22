using Formattica.Models;
using Microsoft.AspNetCore.Http;

namespace Formattica.Service.IService
{
    public interface IConversionService
    {
        Task<(byte[] ConvertedBytes, string? ContentType, string? FileExtension)> ConvertImage(IFormFile file, string targetFormat);
        Task<string> GenerateClass(string databaseType, string targetLanguage, string className,string inputString);
    }
}
