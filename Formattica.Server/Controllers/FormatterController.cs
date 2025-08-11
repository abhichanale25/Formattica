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
    [RequestSizeLimit(20_000_000)] // 20 MB
    public IActionResult FormatLargeJson([FromBody] FormatRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Content))
            return BadRequest("Content is required.");

        if (request.FormatType?.ToUpper() != "JSON")
            return BadRequest("Only 'JSON' format is supported in this test.");

        try
        {
            var parsed = JToken.Parse(request.Content);
            var formatted = parsed.ToString(Formatting.Indented);

            return Ok(new
            {
                Message = "Formatted successfully",
                Length = formatted.Length,
                Preview = formatted
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Error = $"Formatting failed: {ex.Message}"
            });
        }
    }

        public class FormatRequest
        {
            public string? Content { get; set; }
            public string? FormatType { get; set; }
        }


        /*[HttpPost("format-json")]
        public IActionResult FormatJson([FromBody] JsonInputModel model)
        {
            var result = _formatterService.FormatJson(model.JsonInput);

            return Ok(new
            {
                OriginalJson = result.OriginalJson,
                FormattedJson = result.FormattedJson
            });
        }*/


    }
}
