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
using RevenueRec.Services.Interfaces;

namespace RevenueRec.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndividualClientsController : ControllerBase
    {
        private readonly s28786Context _context;
        private readonly IIndividualClientService _service;

        public IndividualClientsController(s28786Context context, IIndividualClientService service)
        {
            _context = context;
            _service = service;
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIndividualClient(IndividualClientRequestDto individualClientRequestDto)
        {
            if (!IndividualClientExists(individualClientRequestDto.IdClient))
            {
                return NotFound();
            }

            var res = await _service.UpdateIndividualClient(individualClientRequestDto);
            return Ok(res);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> PostIndividualClient(IndividualClientRequestDto individualClientRequestDto)
        {
            if (DuplicatePESEL(individualClientRequestDto.PESEL))
            {
                return BadRequest("PESEL duplicate");
            }
            var res = await _service.AddIndividualClient(individualClientRequestDto);
            return Ok(res);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIndividualClient(int id)
        {
            await _service.SoftDeleteIndividualClient(id);

            return NoContent();
        }

        private bool IndividualClientExists(int id)
        {
            return _context.IndividualClients.Any(e => e.IdClient == id);
        }

        private bool DuplicatePESEL(string pesel)
        {
            return _context.IndividualClients.Any(e => e.PESEL == pesel);
        }
    }
}