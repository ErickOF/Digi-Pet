using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Entities;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles =Role.Admin)]
    public class AdminsController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdminsController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("blockWalker/{id}")]
        public async Task<IActionResult> BlockWalker(int id)
        {
            var res = await _userService.BlockWalker(id);
            if (res)
            {
                return Ok();
            }
            else
            {
                return BadRequest(new { message = "error" });
            }
        }
        [HttpPost("unblockWalker/{id}")]
        public async Task<IActionResult> UnBlockWalker(int id)
        {
            var res = await _userService.UnBlockWalker(id);
            if (res)
            {
                return Ok();
            }
            else
            {
                return BadRequest(new { message = "error" });
            }
        }
    }
}