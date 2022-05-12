using BlogVisualStudio.Attributes;
using BlogVisualStudio.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogVisualStudio.Controller
{
    [ApiController]
    [Route("")]
    [Authorize]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [ApiKeyAtribute]
        public async Task<IActionResult> Get()
        {
            return Ok(new ResultViewModel<string>("very nice", null));
        }
    }
}