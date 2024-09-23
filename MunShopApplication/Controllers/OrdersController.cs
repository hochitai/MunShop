using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MunShopApplication.Entities;
using MunShopApplication.Services;

namespace MunShopApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Order order)
        {
            var result = await _orderService.Add(order);

            if (result == null)
            {
                return BadRequest("Can not insert order");
            }

            return Ok(result);
        }

        [HttpPatch("{orderId}")]
        public async Task<IActionResult> Cancel([FromRoute] Guid orderId)
        {
            var result = await _orderService.Cancel(orderId);

            if (result == false)
            {
                return BadRequest("Can not cancel order");
            }

            return NoContent();
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> FindById([FromRoute] Guid orderId)
        {
            var result = await _orderService.FindByID(orderId);

            if (result == null)
            {
                return BadRequest("Can not get order");
            }

            return Ok(result);
        }
    }
}
