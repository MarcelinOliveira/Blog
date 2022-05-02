using BlogEF.Data;
using BlogEF.Models;
using BlogVisualStudio.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogVisualStudio.Controller
{
    [ApiController]
    
    public class CategoryController : ControllerBase
    {   
        [HttpGet("v1/categories")] //Get List
        public async Task<IActionResult> GetAsync(
            [FromServices]VSBlogDataContext context)
        {
            try
            {
                var categories = await context.Categories.ToListAsync();
                if(categories == null) 
                    return NotFound("Não foram encontradas categorias");
                return Ok(categories);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Não foi possivel ler as categorias");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Falha interna no servidor");
            }
        }

        [HttpGet("v1/categories/{id:int}")] //Get One
        public async Task<IActionResult> GetByIDAsync(
            [FromRoute] int id ,
            [FromServices] VSBlogDataContext context)
        {
            try
            {
                var categorie = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if(categorie == null)
                    return StatusCode(500, "Não foi possivel encontrar está categoria");
                return Ok(categorie);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Não foi possivel ler a categoria");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Falha interna no servidor");
            }
        }

        [HttpPost("v1/categories/")] //Post a Categorie
        public async Task<IActionResult> PostAsync
            ( 
            [FromServices] VSBlogDataContext context ,
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
                return Created($"v1/categories/{category.Id}", category);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Não foi possivel incluir a categoria");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Falha interna no servidor");
            }
        }

        [HttpPut("v1/categories/{id:int}")] //Update A Categorie
        public async Task<IActionResult> PutAsync(
            [FromServices] VSBlogDataContext context ,
            [FromRoute] int id,
            [FromBody] EditorCategoryViewModel model)
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
                   if (category == null)
                    return StatusCode(500, "Não foi possivel localizar a categoria");

                category.Slug = model.Slug;
                category.Name = model.Name;

                context.Categories.Update(category);
                await context.SaveChangesAsync();

                return Ok(category);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Não foi possivel Atualizar a categoria");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Falha interna no servidor");
            }
        }

        [HttpDelete("v1/categories/{id:int}")] //Delete a Categorie
        public async Task<IActionResult> DeleteAsync(
            [FromServices] VSBlogDataContext context,
            [FromRoute]int? id
            )
        {
            try
            {
                var delete = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (delete.Id == null)
                    return NotFound("Não foi possivel localizar está categoria");
                var name = delete.Name;

                context.Categories.Remove(delete);
                await context.SaveChangesAsync();
                return Ok($"The Category {name}, has been deleted");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Não foi possivel Deletar a categoria");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Falha interna no servidor");
            }
        }

    }
}
