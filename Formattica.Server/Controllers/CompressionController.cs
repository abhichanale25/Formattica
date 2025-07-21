using Formattica.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace Formattica.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompressionController : ControllerBase
    {
        private readonly ICompressionService _compressionService;
        public CompressionController(ICompressionService compressionService)
        {
            _compressionService = compressionService;
        }

        [HttpPost("compress")]
        public async Task<IActionResult> Compress(IFormFile file)
        {
            if(file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            try
            {
                if(ext == ".pdf")
                {
                    var pdfBytes = await _compressionService.CompressPdf(file);
                    if(pdfBytes == null || pdfBytes.Length == 0)
                        return BadRequest("Invalid or unsupported PDF file.");

                    return File(pdfBytes, "application/pdf", $"compressed_{file.FileName}");
                }
                else
                {
                    var result = await _compressionService.CompressFile(file);
                    return File(result.CompressedBytes, result.ContentType, result.FileName);
                }
            }
            catch(NotSupportedException ex)
            {
                return BadRequest($"Unsupported file type: {ext}. {ex.Message}");
            }
            catch(Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the file. " + ex.Message);
            }
        }


/*        [HttpPost("compress")]
        public async Task<IActionResult> CompressFile(IFormFile file,string fileType)
        {
            if(file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            try
            {
                var result = await _compressionService.CompressFile(file);
                return File(result.CompressedBytes, result.ContentType, result.FileName);
            }
            catch(NotSupportedException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("compress-pdf")]
        public async Task<IActionResult> CompressPdf(IFormFile file)
        {
            var result = await _compressionService.CompressPdf(file);

            if(result == null)
                return BadRequest("Invalid or unsupported PDF file.");

            return File(result, "application/pdf", $"compressed_{file.FileName}");

        }*/
    }
}
