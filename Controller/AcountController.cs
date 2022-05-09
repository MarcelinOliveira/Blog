using BlogEF.Data;
using BlogVisualStudio.Extensions;
using BlogVisualStudio.Models;
using BlogVisualStudio.Services;
using BlogVisualStudio.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogVisualStudio.Controller;

[Authorize]
[ApiController]
[Route("v1")]
public class AcountController : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("login")]
    public IActionResult Login([FromServices] TokenService tokenService)
    {
        string token = tokenService.GenerateToken(null);
        return Ok(token);
    }

    [HttpPost("v1/account")]
    public async Task<IActionResult> Post(
        [FromBody] RegisterViewModel model
        , [FromServices] VSBlogDataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
        var user = new User()
        {
            Name = model.Name,
            Email = model.Email,
            Slug = model.Email.Replace("@", "_").Replace(".", "_")
        };
        return Ok();
    }
}