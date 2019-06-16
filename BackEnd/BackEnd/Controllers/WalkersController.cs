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
        public async Task<ActionResult<IEnumerable<ReturnWalker>>> GetWalker()
        {
            return await _repository.GetAllWalkers();
        }
        [Authorize(Roles ="Admin,Walker")]
        // GET: api/Walkers/5
        [HttpGet("getProfile")]
        public async Task<ActionResult<ReturnWalker>> GetWalkerProfile()
        {
            var username = User.Claims.Where(c=>c.Type==ClaimTypes.Name).FirstOrDefault().Value;
            var role = User.Claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault().Value;

            var walker = await _repository.GetWalkerByUserName(username);

            if (walker == null)
            {
                return NotFound(new { message="not found" });
            }

            var returnWalker = new ReturnWalker(walker);
            
            return returnWalker;
        }


        [AllowAnonymous]
        // POST: api/Walkers
        [HttpPost]
        public async Task<IActionResult> PostWalker([FromBody]WalkerDto walkerDto)
        {

            var result = await _repository.CreateWalker(walkerDto);
            if (result.Item1 != null) return Ok(new { id=result.Item1.Id, UserName=result.Item1.User.Username });
            else return BadRequest(new { message = $"error creating user: {result.Item2}"});
        }
        [Authorize(Roles = Role.Walker)]
        [HttpPost("schedule")]
        public async Task<IActionResult> PostSchedule([FromBody] WeekScheduleDto weekScheduleDto)
        {
            var username = User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var result = await _repository.AddSchedule(weekScheduleDto,username);

            if (result == "success")
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [Authorize(Roles = Role.Walker)]
        [HttpGet("schedule")]
        public async Task<IActionResult> GetSchedule()
        {
            var username = User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var res = await _repository.GetSchedule(username);

            return Ok(res.OrderBy(s=>s.Date));

        }

        [Authorize(Roles =Role.Walker)]
        [HttpGet("history")]
        public async Task<ActionResult<List<WalkInfoDto>>> GetHistory()
        {
            var username = User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var allWalks = await _repository.GetAllWalkerWalks(username);
            allWalks.RemoveAll(w=>w.Begin.AddHours((double)w.Duration)>DateTime.UtcNow);
            allWalks = allWalks.OrderBy(a => a.Begin).ToList();
            return Ok(allWalks);
        }

        [Authorize(Roles = Role.Walker)]
        [HttpGet("upcoming")]
        public async Task<ActionResult<List<WalkInfoDto>>> GetUpcoming()
        {
            var username = User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var allWalks = await _repository.GetAllWalkerWalks(username);
            allWalks.RemoveAll(w => w.Begin < DateTime.UtcNow);
            allWalks = allWalks.OrderBy(a => a.Begin).ToList();
            return Ok(allWalks);
        }

        [Authorize(Roles = Role.Walker)]
        [HttpGet("pendingReport")]
        public async Task<ActionResult<List<WalkInfoDto>>> GetpendingReport()
        {
            var username = User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
            var allWalks = await _repository.GetAllWalkerWalks(username);
            allWalks.RemoveAll(w => w.Report!=null || w.Begin.AddHours((double)w.Duration)>DateTime.UtcNow);
            allWalks = allWalks.OrderByDescending(a => a.Begin).ToList();
            return Ok(allWalks);
        }

        [Authorize(Roles = Role.Walker)]
        [HttpPost("postReport")]
        public async Task<IActionResult> PostReport([FromBody] ReportWalk report)
        {
            var username = User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

            var walker = await _repository.GetWalkerByUserName(username);
            if (!walker.Walks.Select(w => w.Id).Contains(report.WalkId))
                return BadRequest(new { message = "not your walk" });

            var result =await _repository.AddReport(report);
            if (!result.Item1)
            {
                return BadRequest(new { message = result.Item2 });
            }

            return Ok(new { message = result.Item2 });
        }


    }
}
