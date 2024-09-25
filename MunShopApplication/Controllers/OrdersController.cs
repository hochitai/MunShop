using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MunShopApplication.Entities;
using MunShopApplication.Repository;
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

        [Authorize]
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

        [Authorize(Policy = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Order order)
        {
            var result = await _orderService.Update(order);

            if (result == null)
            {
                return BadRequest("Can not update order");
            }

            return Ok(result);
        }

        [Authorize(Policy = "Admin")]
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

        [Authorize]
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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Find([FromQuery] int skip, 
            [FromQuery] int take,
            [FromQuery(Name = "begin_date")] DateTime beginDate,
            [FromQuery(Name = "end_date")] DateTime endDate)
        {

            var result = await _orderService.Find(skip, take, beginDate, endDate);

            if (result == null)
            {
                return BadRequest("Can not get order");
            }

            return Ok(result);
        }
    }
}
