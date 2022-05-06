using BlogEF.Data;
using BlogVisualStudio.Extensions;
using BlogVisualStudio.Models;
using BlogVisualStudio.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace BlogVisualStudio.Controller
{
    [ApiController]
    [Route("v1/categories")]
    public class CategoryController : ControllerBase
    {
        [HttpGet("")] //Get List
        public async Task<IActionResult> GetAsync(
            [FromServices] VSBlogDataContext context)
        {
            try
            {
                var categories = await context.Categories.ToListAsync();
                if (categories == null)
                    return NotFound("Does not found any categories");
                return Ok(new ResultViewModel<List<Category>>(categories));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("1RGA - Unable to read this categories"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("05GA - Internal Failure Server"));
            }
        }

        [HttpGet("{id:int}")] //Get One
        public async Task<IActionResult> GetByIdAsync(
            [FromRoute] int id,
            [FromServices] VSBlogDataContext context)
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
                return category == null
                    ? StatusCode(500, new ResultViewModel<Category>($"13FS - Unable to find the Category {id}"))
                    : Ok(new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Category>($"13GS - Unable to read the Category {id}"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>($"05GS - Internal server Fail"));
            }
        }

        [HttpPost("")] //Post a Category
        public async Task<IActionResult> PostAsync
        (
            [FromServices] VSBlogDataContext context,
            [FromBody] EditorCategoryViewModel model
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));
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
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("12PC - Unable to include this category"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("05PC - Internal Failure Server"));
            }
        }

        [HttpPut("{id:int}")] //Update A Category
        public async Task<IActionResult> PutAsync(
            [FromServices] VSBlogDataContext context,
            [FromRoute] int id,
            [FromBody] EditorCategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                    return StatusCode(500, new ResultViewModel<Category>($"14GS - Unable to find the {id} Category"));

                category.Slug = model.Slug;
                category.Name = model.Name;

                context.Categories.Update(category);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Category>($"14PU - Unable to update the {id} Category"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("05PU - Internal Failure Server"));
            }
        }

        [HttpDelete("{id:int}")] //Delete a Category
        public async Task<IActionResult> DeleteAsync(
            [FromServices] VSBlogDataContext context,
            [FromRoute] int? id
        )
        {
            try
            {
                var delete = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (delete?.Id == null)
                    return NotFound(new ResultViewModel<Category>($"11GS - Unable to find Category {id}"));
                var name = delete.Name;

                context.Categories.Remove(delete);
                await context.SaveChangesAsync();
                return Ok($"The Category {name}, has been deleted");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Category>($"11DS - Unable to Delete Category {id}"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("05DS - Internal Failure Server"));
            }
        }
    }
}