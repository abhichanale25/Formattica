using Formattica.Models;
using Microsoft.AspNetCore.Http;

namespace Formattica.Service.IService
{
    public interface ICompressionService
    {
        Task<(byte[] CompressedBytes, string ContentType, string FileName)> CompressFile(IFormFile file);
        Task<byte[]> CompressPdf(IFormFile file);
    }
}
