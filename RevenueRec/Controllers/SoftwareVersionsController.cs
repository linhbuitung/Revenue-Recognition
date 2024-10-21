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
    public class SoftwareVersionsController : ControllerBase
    {
        private readonly s28786Context _context;
        private readonly ISoftwareService _softwareService;

        public SoftwareVersionsController(s28786Context context, ISoftwareService softwareService)
        {
            _context = context;
            _softwareService = softwareService;
        }

        // GET: api/SoftwareVersions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SoftwareVersion>>> GetSoftwareVersions()
        {
            return await _context.SoftwareVersions.ToListAsync();
        }

        // GET: api/SoftwareVersions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SoftwareVersion>> GetSoftwareVersion(int id)
        {
            var softwareVersion = await _context.SoftwareVersions.FindAsync(id);

            if (softwareVersion == null)
            {
                return NotFound();
            }

            return softwareVersion;
        }

        [HttpPost]
        public async Task<IActionResult> PostSoftwareVersion(SoftwareVersionRequestDto softwareVersion)
        {
            //check if software exist
            if (!_context.SoftwareSystems.Any(x => x.IdSoftwareSystem == softwareVersion.IdSoftwareSystem))
            {
                return BadRequest("Software does not exist");
            }
            SoftwareVersionResponseDto res = await _softwareService.CreateSoftwareVersion(softwareVersion);
            return Ok(res);
        }

        // DELETE: api/SoftwareVersions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSoftwareVersion(int id)
        {
            var softwareVersion = await _context.SoftwareVersions.FindAsync(id);
            if (softwareVersion == null)
            {
                return NotFound();
            }

            _context.SoftwareVersions.Remove(softwareVersion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SoftwareVersionExists(int id)
        {
            return _context.SoftwareVersions.Any(e => e.IdSoftwareVersion == id);
        }
    }
}