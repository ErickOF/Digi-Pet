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

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OwnersController : ControllerBase
    {
        private readonly IRepository _repository;

        public OwnersController(IRepository repository)
        {
 
            _repository = repository;
        }

        // GET: api/Owners
        [Authorize(Roles=Role.Admin)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Petowner>>> GetOwner()
        {
            var owners =await _repository.GetOwners();
            return Ok(owners);
        }
        [Authorize(Roles = "Admin,PetOwner")]
        // GET: api/Owners/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OwnerDto>> GetOwner(int id)
        {
            var username = User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var role = User.Claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault().Value;

            var owner = await _repository.GetOwner(id);
            if (owner == null)
            {
                return NotFound();
            }

            if (owner.User.Username != username && role != Role.Admin)
            {
                return Unauthorized();
            }
            var ownerDto = new OwnerDto
            {
                Name = owner.User.FirstName,
                LastName = owner.User.LastName,
                Email = owner.User.Email,
                Email2 = owner.User.Email2,
                Mobile = owner.User.Mobile,
                Province = owner.User.Province,
                Canton = owner.User.Canton,
                Description = owner.User.Description,
                Pets = owner.Pets.Select(p => new PetDto {Id=p.Id,Name=p.Name,Race=p.Race,Age=p.Age,Size=p.Size,Description=p.Description,Photos=p.Photos }).ToList()
                
            };
            return ownerDto;
        }


        [AllowAnonymous]
        // POST: api/Walkers
        [HttpPost]
        public async Task<IActionResult> PostOwner([FromBody]OwnerDto ownerDto)
        {

            var result = await _repository.CreateOwner(ownerDto);
            if (result.Item1 != null) return Ok(new { id = result.Item1.Id, UserName = result.Item1.User.Username });
            else return BadRequest(new { message = $"error creating user: {result.Item2}" });
        }

    }
}
