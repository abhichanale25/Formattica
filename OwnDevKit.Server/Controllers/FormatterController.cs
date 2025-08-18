using Formattica.Models;
using Formattica.Models.Formatter;
using Formattica.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Formattica.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormatterController : ControllerBase
    {
        private readonly IFormatterService _formatterService;

        public FormatterController(IFormatterService formatterService)
        {
            _formatterService = formatterService;
        }


        [HttpPost("format")]
        [RequestSizeLimit(20_000_000)]
        public IActionResult FormatContent([FromBody] FormatInputModel formatInputModel)
        {
            if(string.IsNullOrWhiteSpace(formatInputModel?.Content))
                return BadRequest(new { Error = "Content is required." });

            if(string.IsNullOrWhiteSpace(formatInputModel.FormatType))
                return BadRequest(new { Error = "FormatType is required. Use JSON, XML, or SQL." });

            try
            {
                var result = _formatterService.FormatContent(formatInputModel);

                if(result.Formatted.StartsWith("Unsupported format type") ||
                    result.Formatted.StartsWith("Invalid JSON") ||
                    result.Formatted.StartsWith("Invalid XML") ||
                    result.Formatted.StartsWith("Invalid SQL"))
                {
                    return BadRequest(new
                    {
                        Error = result.Formatted
                    });
                }

                return Ok(new
                {
                    Message = "Formatted successfully",
                    Length = result.Formatted.Length,
                    Original = result.Original,
                    Preview = result.Formatted
                });
            }
            catch(Exception ex)
            {
                return BadRequest(new
                {
                    Error = $"Formatting failed: {ex.Message}"
                });
            }
        }
    }
}
