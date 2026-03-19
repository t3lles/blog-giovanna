using BlogGiovanna.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogGiovanna.Models;
using BlogGiovanna.ViewModels;
using BlogGiovanna.Extensions;

namespace BlogGiovanna.Controllers
{
    public class CategoryController : ControllerBase
    {
        [HttpGet ("v1/categories")]
        public async Task<IActionResult> GetAsync(
            [FromServices] BlogDataContext context)
        {
            try
            {
                var categories = await context.Categories.ToListAsync();
                return Ok(new ResultViewModel<List<Category>>(categories));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("Falha interna no servidor."));
            }
        }

        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync(
          [FromServices] BlogDataContext context,
          [FromRoute] int id)
        {
           try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado."));
                return Ok(new ResultViewModel<Category>(category));
            }
            catch 
            {
                return StatusCode(500, new ResultViewModel<Category>("Falha interna no servidor."));
            }
        }


        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync(
          [FromServices] BlogDataContext context,
          [FromBody] EditorCategoryViewModel model)
        {

            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

            try
            {
                var category = new Category
                {
                    Id = 0,
                    Name = model.Name,
                    Slug = model.Slug.ToLower()
                };

                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();

                return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Não foi possível incluir categoria.");
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("Falha interna no servidor."));
            }
        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync(
         [FromServices] BlogDataContext context,
         [FromRoute] int id ,
         [FromBody] EditorCategoryViewModel model)
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                    return NotFound();

                category.Name = model.Name;
                category.Slug = model.Slug;

                context.Categories.Update(category);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(category));
            }

            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("Não foi possível alterar a categoria."));
            }
            catch 
            {
                return StatusCode(500, new ResultViewModel<Category>("Falha interna no servidor."));
            }
        }

        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> DeleteAsync(
        [FromServices] BlogDataContext context,
        [FromRoute] int id)
        {
            try
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado."));

                context.Categories.Remove(category);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("Não foi possível deletar a categoria."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("Falha interna no servidor."));
            }
        }

        }
    }
