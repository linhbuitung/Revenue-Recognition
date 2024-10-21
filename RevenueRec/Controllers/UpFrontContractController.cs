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
    public class UpFrontContractController : ControllerBase
    {
        private readonly s28786Context _context;
        private readonly IUpFrontContractService _service;

        public UpFrontContractController(s28786Context context, IUpFrontContractService service)
        {
            _context = context;
            _service = service;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UpFrontContract>>> GetUpFrontContracts()
        {
            return await _context.UpFrontContracts.ToListAsync();
        }

        // GET: api/Subscriptions/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<UpFrontContract>> GetUpFrontContract(int id)
        {
            var contracts = await _context.UpFrontContracts.FindAsync(id);

            if (contracts == null)
            {
                return NotFound();
            }

            return contracts;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostUpFrontContract(UpFrontContractCreateRequestDto upFrontContractCreateRequestDto)
        {
            if (upFrontContractCreateRequestDto == null)
            {
                return BadRequest();
            }

            if (!ClientExists(upFrontContractCreateRequestDto.IdClient))
            {
                return NotFound("Client not found");
            }
            if (!SoftwareSystemExists(upFrontContractCreateRequestDto.IdSoftwareSystem))
            {
                return NotFound("Software System not found");
            }
            if (!SoftwareVersionExists(upFrontContractCreateRequestDto.IdSoftwareSystem, upFrontContractCreateRequestDto.IdSoftwareVersion))
            {
                return NotFound("Software Version not found");
            }
            var res = await _service.AddContract(upFrontContractCreateRequestDto);
            return Ok(res);
        }

        [Authorize]
        [HttpPost("payment/subscription")]
        public async Task<IActionResult> MakePayment(PaymentForUpFrontRequestDto paymentDto)
        {
            if (!ClientExists(paymentDto.IdClient))
            {
                return NotFound("Client not found");
            }
            if (!UpFrontContractExists(paymentDto.IdUpFrontContract))
            {
                return NotFound("Contract not found");
            }
            var res = await _service.MakePaymentToUpFrontContract(paymentDto);
            return Ok(res);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUpFrontContract(int id)
        {
            var upFrontContract = await _context.UpFrontContracts.FindAsync(id);
            if (upFrontContract == null)
            {
                return NotFound();
            }

            await _service.DeleteContract(id);

            return NoContent();
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.IdClient == id);
        }

        private bool SoftwareSystemExists(int id)
        {
            return _context.SoftwareSystems.Any(e => e.IdSoftwareSystem == id);
        }

        private bool SoftwareVersionExists(int idSoftware, int idVersion)
        {
            return _context.SoftwareVersions.Any(e => e.IdSoftwareSystem == idSoftware && e.IdSoftwareVersion == idVersion);
        }

        private bool UpFrontContractExists(int idContract)
        {
            return _context.UpFrontContracts.Any(e => e.IdUpFrontContract == idContract);
        }
    }
}