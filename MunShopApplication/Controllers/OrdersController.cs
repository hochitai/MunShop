﻿using Microsoft.AspNetCore.Http;
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
                return BadRequest("Can not create order");
            }

            return Ok(result);
        }
    }
}
