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
    public class SubscriptionsController : ControllerBase
    {
        private readonly s28786Context _context;
        private readonly ISubscriptionService _service;

        public SubscriptionsController(s28786Context context, ISubscriptionService service)
        {
            _context = context;
            _service = service;
        }

        // GET: api/Subscriptions
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subscription>>> GetSubscriptions()
        {
            return await _context.Subscriptions.ToListAsync();
        }

        // GET: api/Subscriptions/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Subscription>> GetSubscription(int id)
        {
            var subscription = await _context.Subscriptions.FindAsync(id);

            if (subscription == null)
            {
                return NotFound();
            }

            return subscription;
        }

        // POST: api/Subscriptions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostSubscription(SubscriptionRequestDto subscriptionDto)
        {
            if (subscriptionDto == null)
            {
                return BadRequest();
            }

            if (!ClientExists(subscriptionDto.IdClient))
            {
                return NotFound("Client not found");
            }
            if (!SoftwareSystemExists(subscriptionDto.IdSoftwareSystem))
            {
                return NotFound("Software System not found");
            }
            var res = await _service.AddSubscription(subscriptionDto);

            return Ok(res);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscription(int id)
        {
            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }

            _context.Subscriptions.Remove(subscription);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize]
        [HttpPost("payment/subscription")]
        public async Task<IActionResult> MakePayment(PaymentForSubscriptionRequestDto paymentDto)
        {
            paymentDto.PaymentDate = DateTime.Now;
            if (!ClientExists(paymentDto.IdClient))
            {
                return NotFound("Client not found");
            }
            if (!SubscriptionExists(paymentDto.IdSubscription))
            {
                return NotFound("Subscription not found");
            }
            var res = await _service.MakePaymentToSubscription(paymentDto);
            return Ok(res);
        }

        private bool SubscriptionExists(int id)
        {
            return _context.Subscriptions.Any(e => e.IdSubscription == id);
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.IdClient == id);
        }

        private bool SoftwareSystemExists(int id)
        {
            return _context.SoftwareSystems.Any(e => e.IdSoftwareSystem == id);
        }
    }
}