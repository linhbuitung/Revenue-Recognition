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
    public class CompanyClientsController : ControllerBase
    {
        private readonly s28786Context _context;
        private readonly ICompanyClientService _service;

        public CompanyClientsController(s28786Context context, ICompanyClientService service)
        {
            _context = context;
            _service = service;
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompanyClient(CompanyClientRequestDto companyClientRequestDto)
        {
            if (!CompanyClientExists(companyClientRequestDto.IdClient))
            {
                return NotFound();
            }

            var res = await _service.UpdateCompanyClient(companyClientRequestDto);
            return Ok(res);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> PostCompanyClient(CompanyClientRequestDto companyClientRequestDto)
        {
            if (DuplicateKRS(companyClientRequestDto.KRS))
            {
                return BadRequest("KRS duplicate");
            }
            var res = await _service.AddCompanyClient(companyClientRequestDto);

            return Ok(res);
        }

        private bool CompanyClientExists(int id)
        {
            return _context.CompanyClients.Any(e => e.IdClient == id);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> getAllClients()
        {
            var clients = _context.Clients.ToList();
            return Ok(clients);
        }

        private bool DuplicateKRS(string krs)
        {
            return _context.CompanyClients.Any(e => e.KRS == krs);
        }
    }
}