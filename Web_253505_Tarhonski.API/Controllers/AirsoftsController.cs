using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_253505_Tarhonski.API.Data;
using Web_253505_Tarhonski.API.Services;
using Web_253505_Tarhonski.Domain.Entities;
using Web_253505_Tarhonski.Domain.Models;

namespace Web_253505_Tarhonski.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirsoftsController : ControllerBase
    {
        private readonly IAirsoftService _airsoftService;

        public AirsoftsController(IAirsoftService airsoftService)
        {
            _airsoftService = airsoftService;
        }

        // GET: api/Airsofts
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ResponseData<ListModel<Airsoft>>>> GetAirsofts([FromQuery] string? category, [FromQuery] int page = 1, [FromQuery] int pageSize = 6)
        {
            var result = await _airsoftService.GetAirsoftListAsync(category, page, pageSize);
            return Ok(result);
        }

        // GET: api/Airsofts/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseData<Airsoft>>> GetAirsoft(Guid id)
        {
            var result = await _airsoftService.GetAirsoftByIdAsync(id);
            if (!result.Successfull)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // PUT: api/Airsofts/5
        [HttpPut("{id}")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> PutAirsoft(Guid id, Airsoft airsoft)
        {
            await _airsoftService.UpdateAirsoftAsync(id, airsoft);
            return NoContent();
        }

        // POST: api/Airsofts
        [HttpPost]
        [Authorize(Policy = "admin")]
        public async Task<ActionResult<ResponseData<Airsoft>>> PostAirsoft(Airsoft airsoft)
        {
            var result = await _airsoftService.CreateAirsoftAsync(airsoft);
            if (!result.Successfull)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result);
        }

        // DELETE: api/Airsofts/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> DeleteAirsoft(Guid id)
        {
            await _airsoftService.DeleteAirsoftAsync(id);
            return NoContent();
        }
    }
}
