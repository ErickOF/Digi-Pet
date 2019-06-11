using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Dtos;
using WebApi.Entities;
using WebApi.Model;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.View
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WalkersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository _repository;

        public WalkersController(ApplicationDbContext context, IRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        // GET: api/Walkers
        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Walker>>> GetWalker()
        {
            return await _context.Walker.ToListAsync();
        }
        [Authorize(Roles ="Admin,Walker")]
        // GET: api/Walkers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WalkerDto>> GetWalker(int id)
        {
            var username = User.Claims.Where(c=>c.Type==ClaimTypes.Name).FirstOrDefault().Value;
            var role = User.Claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault().Value;
            
            var walker = await _context.Walker.Include(w=>w.User).FirstOrDefaultAsync(u=>u.Id==id);
            if (walker.User.Username != username && role != Role.Admin)
            {
                return Unauthorized();
            }
            var walkerDto = new WalkerDto
            {
                SchoolId = walker.User.Username,
                Name = walker.User.FirstName,
                LastName = walker.User.LastName,
                Email = walker.User.Email,
                Email2 = walker.User.Email2,
                Mobile = walker.User.Mobile,
                University = walker.University,
                Province = walker.User.Province,
                Canton = walker.User.Canton,
                DoesOtherProvinces = walker.DoesOtherProvinces,
                OtherProvinces = walker.OtherProvinces,
                Description = walker.User.Description
            }; 

            if (walker == null)
            {
                return NotFound();
            }

            return walkerDto;
        }


        [AllowAnonymous]
        // POST: api/Walkers
        [HttpPost]
        public async Task<IActionResult> PostWalker([FromForm]WalkerDto walkerDto)
        {

            var result = await _repository.CreateWalker(walkerDto);
            if (result.Item1 != null) return Ok(new { id=result.Item1.Id, UserName=result.Item1.User.Username });
            else return BadRequest(new { message = $"error creating user: {result.Item2}"});
        }

        private bool WalkerExists(int id)
        {
            return _context.Walker.Any(e => e.Id == id);
        }
    }
}
