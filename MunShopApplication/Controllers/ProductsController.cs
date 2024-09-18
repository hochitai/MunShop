using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MunShopApplication.Entities;
using MunShopApplication.Services;

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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _productService.GetAll();

            if (result == null)
            {
                return BadRequest("Can not get Products");
            }
            return Ok(result);
        }

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
