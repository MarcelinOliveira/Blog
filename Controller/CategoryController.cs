using BlogEF.Data;
using BlogVisualStudio.Models;
using BlogVisualStudio.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogVisualStudio.Controller
{
    [ApiController]
    [Authorize]
    [Route("v1/categories")]
    public class CategoryController : ControllerBase
    {
        [HttpGet("")]
        [Authorize(Roles = "user")] //Get List
        public async Task<IActionResult> GetAsync(
            [FromServices] VSBlogDataContext context)
        {
            try
            {
                var categories = await context.Categories.ToListAsync();
                var categoriesString = categories.ToArray()
                    .Select(category =>
                        $" Category ID: {category.Id} \n Category Name: {category.Name} \n Category Posts: {category.Posts} \n Category Slug: {category.Slug} \n\r")
                    .Aggregate("", (current, stringbuild) => current + stringbuild);
                return (Ok(new ResultViewModel<string>(categoriesString, null)));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<string>("1RGA - Unable to read this categories"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("05GA - Internal Failure Server"));
            }
        }

        [HttpGet("{id:int}")] //Get One
        [Authorize(Roles = "user")]
        [Authorize(Roles = "admin")]
        [Authorize(Roles = "author")]
        public async Task<IActionResult> GetByIdAsync(
            [FromRoute] int id,
            [FromServices] VSBlogDataContext context)
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
                return category == null
                    ? StatusCode(500, new ResultViewModel<string>($"Unable to find the Category {id}"))
                    : Ok(new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<string>($"13GS - Unable to read the Category {id}"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>($"05GS - Internal server Fail"));
            }
        }

        [HttpPost("Post")]
        [Authorize(Roles = "admin")] //Post a Category
        public async Task<IActionResult> PostAsync
        (
            [FromServices] VSBlogDataContext context,
            [FromBody] EditorCategoryViewModel model
        )
        {
            try
            {
                var category = new Category()
                {
                    Id = 0,
                    Name = model.Name,
                    Slug = model.Slug,
                    Posts = null
                };
                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();
                return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<string>("12PC - Unable to include this category"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("05PC - Internal Failure Server"));
            }
        }

        [HttpPut("Put/{id:int}")]
        [Authorize(Roles = "admin")] //Update A Category
        public async Task<IActionResult> PutAsync(
            [FromServices] VSBlogDataContext context,
            [FromRoute] int id,
            [FromBody] EditorCategoryViewModel model)
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                    return StatusCode(500, $"14GS - Unable to find the {id} Category");

                category.Slug = model.Slug;
                category.Name = model.Name;

                context.Categories.Update(category);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<string>($"14PU - Unable to update the {id} Category"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("05PU - Internal Failure Server"));
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "admin")] //Delete a Category
        public async Task<IActionResult> DeleteAsync(
            [FromServices] VSBlogDataContext context,
            [FromRoute] int? id
        )
        {
            try
            {
                var delete = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (delete?.Id == null)
                    return NotFound(new ResultViewModel<string>($"11GS - Unable to find Category {id}"));
                var name = delete.Name;

                context.Categories.Remove(delete);
                await context.SaveChangesAsync();
                return Ok(new ResultViewModel<string>($"The Category {name}, has been deleted", null));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<string>($"11DS - Unable to Delete Category {id}"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<string>("05DS - Internal Failure Server"));
            }
        }
    }
}