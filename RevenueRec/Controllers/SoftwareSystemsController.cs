using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RevenueRec.Context;
using RevenueRec.Models;
using RevenueRec.RequestDtos;
using RevenueRec.ResponseDtos;
using RevenueRec.Services.Interfaces;

namespace RevenueRec.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoftwareSystemsController : ControllerBase
    {
        private readonly s28786Context _context;
        private readonly ISoftwareService _softwareService;

        public SoftwareSystemsController(s28786Context context, ISoftwareService softwareService)
        {
            _context = context;
            _softwareService = softwareService;
        }

        // GET: api/SoftwareSystems

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SoftwareSystem>>> GetSoftwareSystems()
        {
            return await _context.SoftwareSystems.ToListAsync();
        }

        // GET: api/SoftwareSystems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SoftwareSystem>> GetSoftwareSystem(int id)
        {
            var softwareSystem = await _context.SoftwareSystems.FindAsync(id);

            if (softwareSystem == null)
            {
                return NotFound();
            }

            return softwareSystem;
        }

        [HttpPost]
        public async Task<IActionResult> PostSoftwareSystemWithInitVersion(SoftwareSystemAndVersionRequestDto requestDto)
        {
            SoftwareSystemRequestDto softwareSystem = requestDto.SoftwareSystemRequestDto;
            SoftwareVersionRequestDto softwareVersion = requestDto.SoftwareVersionRequestDto;
            //check if category exists
            if (!_context.Categories.Any(x => x.IdCategory == softwareSystem.IdCategory))
            {
                return BadRequest("Category does not exist");
            }
            SoftwareSystemResponseDto res = await _softwareService.CreateSoftwareSystemWithInitVersion(softwareSystem, softwareVersion);

            return Ok(res);
        }

        [HttpPost("/no-init-ver")]
        public async Task<IActionResult> PostSoftwareSystem(SoftwareSystemRequestDto requestDto)
        {
            //check if category exists
            if (!_context.Categories.Any(x => x.IdCategory == requestDto.IdCategory))
            {
                return BadRequest("Category does not exist");
            }
            SoftwareSystemResponseDto res = await _softwareService.CreateSoftwareSystem(requestDto);

            return Ok(res);
        }

        // DELETE: api/SoftwareSystems/5

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSoftwareSystem(int id)
        {
            var softwareSystem = await _context.SoftwareSystems.FindAsync(id);
            if (softwareSystem == null)
            {
                return NotFound();
            }

            _context.SoftwareSystems.Remove(softwareSystem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SoftwareSystemExists(int id)
        {
            return _context.SoftwareSystems.Any(e => e.IdSoftwareSystem == id);
        }
    }
}