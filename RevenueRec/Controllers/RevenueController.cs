using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RevenueRec.Context;
using RevenueRec.Models;
using RevenueRec.RequestDtos;
using RevenueRec.ResponseDtos;
using RevenueRec.Services.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Authorization;

namespace RevenueRec.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevenueController : ControllerBase
    {
        private readonly s28786Context _context;
        private readonly IRevenueService _service;

        public RevenueController(s28786Context context, IRevenueService service)
        {
            _context = context;
            _service = service;
        }

        /*revenue For the entire company.
        For a specific product.
        For a specific currency*/

        [Authorize]
        [HttpGet("/calculate")]
        public async Task<IActionResult> GetRevenue(int? idSoftwareSystem, string? currencyCode)
        {
            //check software system exists
            if (idSoftwareSystem != null)
            {
                if (!SoftwareSystemExists((int)idSoftwareSystem))
                {
                    return NotFound("Software system not exist");
                }
            }
            RevenueResponseDto res = await _service.CalculateRevenue(idSoftwareSystem, currencyCode);

            return Ok(res);
        }

        //[Authorize]
        [HttpGet]
        [Route("/predict")]
        public async Task<IActionResult> GetPredictedRevenue(DateTime timePoint, int? idSoftwareSystem, string? currencyCode)
        {
            if (idSoftwareSystem != null)
            {
                if (!SoftwareSystemExists((int)idSoftwareSystem))
                {
                    return NotFound("Software system not exist");
                }
            }
            RevenueResponseDto res = await _service.PredictRevenue(timePoint, idSoftwareSystem, currencyCode);

            return Ok(res);
        }

        private bool SoftwareSystemExists(int id)
        {
            return _context.SoftwareSystems.Any(e => e.IdSoftwareSystem == id);
        }
    }
}