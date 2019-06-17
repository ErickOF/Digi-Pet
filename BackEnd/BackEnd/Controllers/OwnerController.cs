using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        private readonly IUserService _userService;

        public OwnersController(IRepository repository,IUserService userService)
        {
 
            _repository = repository;
            _userService = userService;
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
                return BadRequest(new { message = "pet not yours!" });
            }
            var result = await _repository.RequestWalk(walkRequestDto);
            if (result.Item1 == null)
            {
                return BadRequest(result.Item2);
            }
            return Ok(result.Item1);
        }

        [Authorize(Roles = Role.Petowner)]
        [HttpGet("history/{petId}")]
        public async Task<ActionResult<List<WalkInfoDto>>> GetHistory(int petId)
        {
            var username = User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var user = await _repository.GetOwnerByUserName(username);
            if (!user.Pets.Select(p => p.Id).Contains(petId)){
                return BadRequest(new { message = "pet not yours" });
            }
            var allWalks = await _repository.GetAllPetWalks(petId);
            allWalks.RemoveAll(w => w.Begin.AddHours((double)w.Duration) > DateTime.UtcNow);
            allWalks = allWalks.OrderBy(a => a.Begin).ToList();
            return Ok(allWalks);
        }

        [Authorize(Roles = Role.Petowner)]
        [HttpGet("upcoming")]
        public async Task<ActionResult<List<WalkInfoDto>>> GetUpcoming()
        {
            var username = User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var allWalks = await _repository.GetAllOwnerWalks(username);
            allWalks.RemoveAll(w => w.Begin < DateTime.UtcNow);
            allWalks=allWalks.OrderBy(a => a.Begin).ToList();
            return Ok(allWalks);
        }

        [Authorize(Roles = Role.Petowner)]
        [HttpGet("pendingReport")]
        public async Task<ActionResult<List<WalkInfoDto>>> GetPendingReport()
        {
            var username = User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var allWalks = await _repository.GetAllOwnerWalks(username);
            allWalks.RemoveAll(w => w.Report!=null &&w.Report.Count!=0);
            allWalks = allWalks.OrderBy(a => a.Begin).ToList();
            return Ok(allWalks);
        }

        /*[Authorize(Roles = Role.Petowner)]
        [HttpGet("pendingReport")]
        public async Task<ActionResult<List<WalkInfoDto>>> GetpendingReport()
        {
            var username = User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var allWalks = await _repository.GetAllWalkerWalks(username);
            allWalks.RemoveAll(w => w.Report != null || w.Begin.AddHours((double)w.Duration) > DateTime.UtcNow);
            return Ok(allWalks);
        }*/

        [Authorize(Roles = Role.Petowner)]
        [HttpGet("report/{walkId}")]
        public async Task<IActionResult> GetReport(int walkId)
        {
            var username = User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            if (!_userService.ValidateIfWalkBelongsToUser(username, walkId))
            {
                return BadRequest(new { mesagge = "not your walk" });
            }
            var report = await _repository.GetReportCard(walkId);
       
            return Ok(report);
        }

        [Authorize(Roles = Role.Petowner)]
        [HttpPost("rate")]
        public async Task<IActionResult> RateTrip([FromBody] Rate rate)
        {
            var username = User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            if (!_userService.ValidateIfWalkBelongsToUser(username, rate.walkId))
            {
                return BadRequest(new { mesagge = "not your walk" });
            }
            var res = await _repository.Rate(rate);
            
            if (!res.Item1) return BadRequest(new { message = res.Item2 });

            return Ok(new { message = res.Item2 });

        }
    }
}
