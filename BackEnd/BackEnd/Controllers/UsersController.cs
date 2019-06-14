using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Entities;
using WebApi.Dtos;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
            
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromForm]AuthenticateDto userParam)
        {
            var result = await _userService.AuthenticateAsync(userParam.UserName, userParam.Password);

            if (result.Item1 == null)
                return BadRequest(new { message = result.Item2 });

            return Ok(result.Item1);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Create([FromForm] User user)
        {

            return Ok();
        }

        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users =  _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user =  _userService.GetById(id);

            if (user == null) {
                return NotFound();
            }

            // only allow admins to access other user records
            var currentUserId = int.Parse(User.Identity.Name);
            if (id != currentUserId && !User.IsInRole(Role.Admin)) {
                return Forbid();
            }

            return Ok(user);
        }
    }
}
