using Formattica.Models.Comparison;
using Formattica.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace Formattica.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComparisonController : ControllerBase
    {
        private readonly IComparisonService _comparisonService;

        public ComparisonController(IComparisonService comparisonService)
        {
            _comparisonService = comparisonService;
        }

        [HttpPost("compare")]
        public IActionResult CompareCode([FromBody] CodeComparisonRequest request)
        {
            var result = _comparisonService.Compare(request.OldCode, request.NewCode);
            return Ok(result);
        }
    }
}
