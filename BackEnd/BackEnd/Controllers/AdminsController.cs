﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Entities;
using WebApi.Model;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles =Role.Admin)]
    public class AdminsController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRepository _repo;

        public AdminsController(IUserService userService, IRepository repository)
        {
            _userService = userService;
            _repo = repository;
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
        [HttpGet("HourPrice")]
        public IActionResult GetHourPrice()
        {
            decimal price = _repo.HourPrice();
            return Ok(new { hourPrice = price });
        }
        [HttpPost("HourPrice")]
        public IActionResult PostHourPrice([FromBody] decimal hourPrice)
        {
            bool result = _repo.PostHourPrice(hourPrice);
            if(result) return Ok(new { hourPrice = hourPrice });
            return BadRequest(new { message="error"});
        }
        [HttpGet("denuncias")]
        public ActionResult<List<Denuncia>> GetDenuncias()
        {
            var res =  _repo.GetDenuncias();
            return Ok(res);
        }
        [HttpDelete("denuncias/{id}")]
        public IActionResult DeleteDenuncia(int id)
        {
            var res = _repo.DeleteDenuncia(id);
            if (res) return Ok(new { message = "ok" });
            else return BadRequest(new { message = "error" });
        }
        #region paraTest
        //solo para test
        [HttpDelete("deleteWalk")]
        public async Task<ActionResult<bool>> DeleteWalk(int id)
        {
            var res = await _repo.DropWalk(id);
            if (res) return Ok(res);
            else return BadRequest(res);
        }

        [HttpPost("modifyDateOfWalk/{walkId}")]
        public async Task<IActionResult> ModifyDateWalk(int walkId,[FromBody] DateTime newDate)
        {
            var res=await _repo.Modify(walkId, newDate);
            if (res) return Ok(new { message = "success" });
            return BadRequest(new { message = "error" });
            
        }
        #endregion

    }
}