using Microsoft.AspNetCore.Mvc;

namespace BlogVisualStudio.Controller
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {   
        [HttpGet("")]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
