using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MunShopApplication.Entities;
using MunShopApplication.Services;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace MunShopApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Find(
            [FromQuery] int skip,
            [FromQuery] int take,
            [FromQuery(Name = "min-price")] float minPrice,
            [FromQuery(Name = "max-price")] float maxPrice,
            [FromQuery] string? name,
            [FromQuery(Name = "category_id")] Guid categoryId)
        {
            var result = await _productService.Find(skip, take, minPrice, maxPrice, name, categoryId);

            if (result == null)
            {
                return BadRequest("Can not get Products");
            }
            return Ok(result);
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Product product)
        {
            var result = await _productService.Add(product);
            if (result == null)
            {
                return BadRequest("Can not insert Products");
            }
            return Ok(result);
        }

        [Authorize(Policy = "Admin")]
        [HttpPut("{productId}")]
        public async Task<IActionResult> Update([FromRoute] Guid productId, [FromBody] Product product)
        {
            product.Id = productId;
            var result = await _productService.Update(product);
            if (result == null)
            {
                return BadRequest("Can not update Products");
            }
            return Ok(result);
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid productId)
        {
            var result = await _productService.Delete(productId);
            if (!result)
            {
                return BadRequest("Can not delete Products");
            }
            return NoContent();
        }
    }
}
