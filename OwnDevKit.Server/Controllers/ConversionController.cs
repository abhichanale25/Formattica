using Formattica.Service.IService;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Formattica.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConversionController : ControllerBase
    {

        private readonly IConversionService _conversionService;

        public ConversionController(IConversionService conversionService)
        {
            _conversionService = conversionService;//?? throw new ArgumentNullException(nameof(conversionService))
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

        [HttpPost("GenerateClass")]
        public async Task<IActionResult> GenerateClass(string databaseType, string targetLanguage,string className, string inputString)
        {
            var validDbTypes = new HashSet<string> { "relational", "document", "keyvaluestore", "columnfamily", "graph" };
            if (string.IsNullOrWhiteSpace(databaseType) || !validDbTypes.Contains(databaseType.ToLower()))
                throw new ArgumentException("Invalid database type. Supported types: Relational, Document, KeyValueStore, ColumnFamily, Graph.");

            var validLanguages = new HashSet<string> {
                "c", "c++", "java", "python", "c#", "go", "rust",
                "swift", "kotlin", "ruby", "dart", "objective c", "scala", "perl"
                    };
            if (string.IsNullOrWhiteSpace(targetLanguage) || !validLanguages.Contains(targetLanguage.ToLower()))
                throw new ArgumentException("Invalid language. Must be one of: " + string.Join(", ", validLanguages));

            if (string.IsNullOrWhiteSpace(className) || !Regex.IsMatch(className, @"^[A-Za-z_][A-Za-z0-9_]*$"))
                throw new ArgumentException("Invalid class name. Must be a valid identifier.");

            if (string.IsNullOrWhiteSpace(inputString) || inputString.Trim().Length < 10)
                throw new ArgumentException("Definition is too short or missing.");

            var result = await _conversionService.GenerateClass(databaseType, targetLanguage, className, inputString);


            return Ok(result);
        }

    }
}
