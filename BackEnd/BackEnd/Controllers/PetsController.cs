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
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Authorize(Roles =Role.Petowner)]
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        private readonly IRepository _repository;

        public PetsController( IRepository repository)
        {
            _repository = repository;
        }
        //GET: api/Pets/ devuelve todas las mascotas del usuario
        [HttpGet]
        public async Task<ActionResult<ICollection<PetDto>>> GetAll(){
            var username = User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

            var owner =await _repository.GetOwnerByUserName(username);

            var pets = owner.Pets.Select(p => new PetDto
            {
                Id = p.Id,
                Name = p.Name,
                Race = p.Race,
                Age = p.Age,
                Size = p.Size,
                Description = p.Description,
                Photos = p.Photos,
                Trips = p.WalksQuant,
                DateCreated = p.DateCreated
            }).ToList();
            return pets;

        }
        // GET: api/Pets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PetDto>> GetPet(int id)
        {
            var username = User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
     
            var owner = await _repository.GetOwnerByUserName(username);
            if (owner == null)
            {
                return NotFound();
            }
            var pet = owner.Pets.FirstOrDefault(p => p.Id == id);

            if (pet == null)
            {
                return NotFound();
            }
            var petDto = new PetDto
            {
                Id = pet.Id,
                Name = pet.Name,
                Race = pet.Race,
                Age = pet.Age,
                Size = pet.Size,
                Description = pet.Description,
                Photos = pet.Photos,
                Trips = pet.WalksQuant,
                DateCreated = pet.DateCreated
            };


            return petDto;
        }
        // POST: api/Pets
        [HttpPost]
        public async Task<IActionResult> PostPet([FromBody]PetDto petDto)
        {
            var username = User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

            var owner = await _repository.GetOwnerByUserName(username);
            if (owner == null)
            {
                return NotFound();
            }

            var result = await _repository.CreatePet(petDto,owner.Id);
            if (result.Item1 != null) return Ok(new { id = result.Item1.Id, Name = result.Item1.Name });
            else return BadRequest(new { message = $"error creating user: {result.Item2}" });
        }
    }
}