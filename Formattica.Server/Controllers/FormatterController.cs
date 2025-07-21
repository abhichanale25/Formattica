using Formattica.Models;
using Formattica.Models.Formatter;
using Formattica.Service.IService;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult FormatContent(string? Content, string? FormatType)
        {
            if(string.IsNullOrWhiteSpace(Content))
                return BadRequest("Content is required.");

            var result = _formatterService.FormatContent(Content, FormatType);
            return Ok(result);
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
