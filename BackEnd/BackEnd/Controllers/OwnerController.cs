﻿using System;
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
        public async Task<ActionResult<IEnumerable<ReturnOwner>>> GetOwner()
        {
            var owners =await _repository.GetOwners();

            return Ok(owners);
        }
        [Authorize(Roles = "Admin,PetOwner")]
        // GET: api/Owners/5
        [HttpGet("getprofile")]
        public async Task<ActionResult<ReturnOwner>> GetOwnerProfile()
        {
            var username = User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var role = User.Claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault().Value;

            var owner = await _repository.GetOwnerByUserName(username);
            if (owner == null)
            {
                return NotFound();
            }
            return owner;
        }


        [AllowAnonymous]
        // POST: api/Owners
        [HttpPost]
        public async Task<IActionResult> PostOwner([FromBody]OwnerDto ownerDto)
        {

            var result = await _repository.CreateOwner(ownerDto);
            if (result.Item1 != null) return Ok(new { id = result.Item1.Id, UserName = result.Item1.User.Username });
            else return BadRequest(new { message = $"error creating user: {result.Item2}" });
        }
        [Authorize(Roles = Role.Petowner)]
        [HttpPost("requestWalk")]
        public async Task<IActionResult> PostWalkRequest([FromBody] WalkRequestDto walkRequestDto)
        {
            var username = User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var owner = await _repository.GetOwnerByUserName(username);
            var petsIds = owner.Pets.Select(p => p.Id);
            if (!petsIds.Contains(walkRequestDto.PetId))
            {
                return BadRequest("pet not yours!");
            }
            var result = await _repository.RequestWalk(walkRequestDto);
            if (result.Item1 == null)
            {
                return BadRequest(result.Item2);
            }
            return Ok(result.Item1);
        }

    }
}
