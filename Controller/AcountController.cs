using BlogEF.Data;
using BlogVisualStudio.Extensions;
using BlogVisualStudio.Models;
using BlogVisualStudio.Services;
using BlogVisualStudio.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace BlogVisualStudio.Controller;

[ApiController]
[Route("v1")]
public class AcountController : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromServices] TokenService tokenService
        , [FromServices] VSBlogDataContext context
        , [FromBody] LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
        var user = await context
            .Users
            .AsNoTracking()
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Email == model.Email);

        if (user == null)
            return StatusCode(401, new ResultViewModel<string>("58BX - User or Password Invalid"));


        if (!PasswordHasher.Verify(user.PasswordHash, model.Password))
            return StatusCode(401, new ResultViewModel<string>("58BX - User or Password Invalid"));
        try
        {
            string token = tokenService.GenerateToken(user);
            return Ok(new ResultViewModel<string>(token, null));
        }
        catch (Exception e)
        {
            return StatusCode(400, new ResultViewModel<string>("05X04 - Internal Failure"));
        }
    }


    [HttpPost("account")]
    public async Task<IActionResult> Post(
        [FromBody] RegisterViewModel model
        , [FromServices] VSBlogDataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));
        try
        {
            var user = new User()
            {
                Name = model.Name,
                Email = model.Email,
                Slug = model.Email.Replace("@", "_").Replace(".", "_")
            };

            var password = PasswordGenerator.Generate(25);
            user.PasswordHash = PasswordHasher.Hash(password);


            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return Ok(new ResultViewModel<dynamic>(new
            {
                user = user.Email, password
            }));
        }
        catch (DbUpdateException d)
        {
            return StatusCode(400,
                new ResultViewModel<string>($"05X99 - {d.InnerException}"));
        }
        catch (Exception e)
        {
            return StatusCode(400, new ResultViewModel<string>("05X04 - Internal Failure"));
        }
    }
}