using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MunShopApplication.Entities;
using MunShopApplication.Services;

namespace MunShopApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var result = await _userService.Add(user);
            if (result == null)
            {
                return BadRequest("Can not register account");
            }
            return Created("", result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var result = await _userService.Login(user);
            if (result == null)
            {
                return BadRequest("Can not login account");
            }
            return Ok(result);
        }
    }
}
