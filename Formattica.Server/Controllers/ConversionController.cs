using Formattica.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace Formattica.Server.Controllers
{
    public class ConversionController : ControllerBase
    {

        private readonly IConversionService _conversionService;

        public ConversionController(IConversionService conversionService)
        {
            _conversionService = conversionService ?? throw new ArgumentNullException(nameof(conversionService));
        }

        [HttpPost("convert")]
        public async Task<IActionResult> ConvertImage(IFormFile file, [FromQuery] string targetFormat)
        {
            var (convertedBytes, contentType, extension) = await _conversionService.ConvertImage(file, targetFormat);

            if(convertedBytes == null)
                return BadRequest("Invalid file or unsupported format.");

            var fileName = $"converted.{extension}";
            return File(convertedBytes, contentType!, fileName);
        }

    }
}
