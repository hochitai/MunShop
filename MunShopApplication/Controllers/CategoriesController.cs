using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MunShopApplication.Entities;
using MunShopApplication.Services;

namespace MunShopApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.GetAll();

            if (result == null)
            {
                return BadRequest("Can not get Categories");
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Category category)
        {
            var result = await _categoryService.Add(category);
            if (result == null) {
                return BadRequest("Can not insert Categories");
            }
            return Ok(result);
        }

        [HttpPut("{categoryId}")]
        public async Task<IActionResult> Update([FromRoute] Guid categoryId, [FromBody] Category category)
        {
            category.Id = categoryId;
            var result = await _categoryService.Update(category);
            if (result == null)
            {
                return BadRequest("Can not update Categories");
            }
            return Ok(result);
        }

        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid categoryId)
        {
            var result = await _categoryService.Delete(categoryId);
            if (!result)
            {
                return BadRequest("Can not delete Categories");
            }
            return NoContent();
        }
    }
}
