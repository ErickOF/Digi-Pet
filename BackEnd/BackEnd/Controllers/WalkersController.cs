using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Dtos;
using WebApi.Model;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.View
{
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Walker>>> GetWalker()
        {
            return await _context.Walker.ToListAsync();
        }

        // GET: api/Walkers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Walker>> GetWalker(int id)
        {
            var walker = await _context.Walker.FindAsync(id);

            if (walker == null)
            {
                return NotFound();
            }

            return walker;
        }

        // PUT: api/Walkers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWalker(int id, Walker walker)
        {
            if (id != walker.Id)
            {
                return BadRequest();
            }

            _context.Entry(walker).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WalkerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Walkers
        [HttpPost]
        public async Task<ActionResult<bool>> PostWalker([FromForm]WalkerDto walkerDto)
        {
            var result = await _repository.CreateWalker(walkerDto);
            if (result) return Ok(result);
            else return BadRequest(result);
        }

        private bool WalkerExists(int id)
        {
            return _context.Walker.Any(e => e.Id == id);
        }
    }
}
