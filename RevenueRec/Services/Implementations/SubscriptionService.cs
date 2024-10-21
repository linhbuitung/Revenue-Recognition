using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RevenueRec.Context;
using RevenueRec.Models;
using RevenueRec.RequestDtos;
using RevenueRec.ResponseDtos;
using RevenueRec.Services.Interfaces;

namespace RevenueRec.Services.Implementations
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly s28786Context _context;

        public SubscriptionService(s28786Context context)
        {
            _context = context;
        }

        public async Task<SubscriptionResponseDto> AddSubscription(SubscriptionRequestDto subscriptionRequestDto)
        {
            if (await IsSoftwareAlreadyBeingUsed(subscriptionRequestDto.IdSoftwareSystem, subscriptionRequestDto.IdClient))
            {
                throw new Exception("The software is already being used by this client.");
            }

            Subscription subscription = new Subscription
            {
                Name = subscriptionRequestDto.Name,
                StartDate = DateTime.Now,
                RenewalPeriod = subscriptionRequestDto.RenewalPeriod,
                EndDate = DateTime.Now.AddMonths((int)subscriptionRequestDto.RenewalPeriod),
                IsCancelled = false,
                Price = subscriptionRequestDto.Price,
                IdClient = subscriptionRequestDto.IdClient,
                IdSoftwareSystem = subscriptionRequestDto.IdSoftwareSystem,
            };

            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();
            Payment payment = new Payment
            {
                Amount = await GetPeriodPrice(subscription.IdSubscription, true),
                PaymentDate = DateTime.Now,
                IdClient = subscription.IdClient,
                IdSubscription = subscription.IdSubscription
            };
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            SubscriptionResponseDto subscriptionResponseDto = new SubscriptionResponseDto
            {
                IdSubscription = subscription.IdSubscription,
                Name = subscription.Name,
                StartDate = subscription.StartDate,
                RenewalPeriod = subscription.RenewalPeriod,
                EndDate = subscription.EndDate,
                IsCancelled = subscription.IsCancelled,
                Price = subscription.Price,
                IdClient = subscription.IdClient,
                IdSoftwareSystem = subscription.IdSoftwareSystem
            };
            return subscriptionResponseDto;
        }

        public async Task<SubscriptionResponseDto> MakePaymentToSubscription(PaymentForSubscriptionRequestDto paymentDto)
        {
            if (!_context.Subscriptions.Any(e => e.IdClient == paymentDto.IdClient && e.IdSubscription == paymentDto.IdSubscription))
            {
                throw new Exception("Subscription does not exits for this client");
            }
            Subscription subscription = _context.Subscriptions.Find(paymentDto.IdSubscription);

            if (subscription.IsCancelled)
            {
                throw new Exception("The subscription is cancelled.");
            }
            //get latest payment for this subscription (we are assured that there is atleast a payment)
            Payment latestPayment = await _context.Payments.Where(p => p.IdSubscription == paymentDto.IdSubscription)
                .OrderByDescending(p => p.PaymentDate).FirstOrDefaultAsync();
            //check if the latest payment is during the last period

            if (latestPayment.PaymentDate >= subscription.EndDate.AddMonths(-(int)subscription.RenewalPeriod)
                && paymentDto.PaymentDate < subscription.EndDate)
            {
                throw new Exception("This period is already paid.");
            }

            if (paymentDto.PaymentDate >= subscription.EndDate.AddDays(10))
            {
                subscription.IsCancelled = true;
                await _context.SaveChangesAsync();
                throw new Exception("Subscription expired");
            }
            if (paymentDto.Amount != await GetPeriodPrice(subscription.IdSubscription, false))
            {
                throw new Exception("Invalid payment amount");
            }
            Payment payment = new Payment
            {
                Amount = paymentDto.Amount,
                PaymentDate = paymentDto.PaymentDate,
                IdClient = subscription.IdClient,
                IdSubscription = subscription.IdSubscription
            };
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            subscription.EndDate = subscription.EndDate.AddMonths((int)subscription.RenewalPeriod);
            await _context.SaveChangesAsync();

            SubscriptionResponseDto subscriptionResponseDto = new SubscriptionResponseDto
            {
                IdSubscription = subscription.IdSubscription,
                Name = subscription.Name,
                StartDate = subscription.StartDate,
                RenewalPeriod = subscription.RenewalPeriod,
                EndDate = subscription.EndDate,
                IsCancelled = subscription.IsCancelled,
                Price = subscription.Price,
                IdClient = subscription.IdClient,
                IdSoftwareSystem = subscription.IdSoftwareSystem
            };
            return subscriptionResponseDto;
        }

        private async Task<bool> IsReturningClient(int clientId, int? contractId, int? subscriptionId)
        {
            var hasContract = false;
            if (contractId != null)
            {
                hasContract = await _context.UpFrontContracts.AnyAsync(c => c.IdClient == clientId && c.IsSigned && c.IdUpFrontContract != contractId);
            }
            else
            {
                hasContract = await _context.UpFrontContracts.AnyAsync(c => c.IdClient == clientId && c.IsSigned);
            }
            var hasSubscription = false;
            if (subscriptionId != null)
            {
                hasSubscription = await _context.Subscriptions.AnyAsync(s => s.IdClient == clientId && s.IdSubscription != subscriptionId);
            }
            else
            {
                hasSubscription = await _context.Subscriptions.AnyAsync(s => s.IdClient == clientId);
            }
            return hasSubscription || hasContract;
        }

        private async Task<bool> IsSoftwareAlreadyBeingUsed(int softwareId, int clientId)
        {
            bool res = await _context.UpFrontContracts.AnyAsync(e => e.IdSoftwareSystem == softwareId && e.IdClient == clientId && e.IsCancelled == false)
                || await _context.Subscriptions.AnyAsync(e => e.IdSoftwareSystem == softwareId && e.IdClient == clientId && e.IsCancelled == false);
            return res; return res;
        }

        private async Task<decimal> GetPeriodPrice(int id, bool isFirst)
        {
            Subscription subscription = await _context.Subscriptions.Where(s => s.IdSubscription == id).FirstOrDefaultAsync();
            var discounts = await _context.Discounts.ToListAsync();
            double highestDiscount = 0;
            if (_context.Discounts.Any(e => !e.IsForUpFront))
            {
                highestDiscount = _context.Discounts.Where(d => !d.IsForUpFront).Max(d => d.Percentage);
            }
            double returnDiscount = 0;
            if (await IsReturningClient(subscription.IdClient, null, id))
            {
                returnDiscount = 0.05;
            }
            decimal discountedPrice = 0;
            if (isFirst)
            {
                discountedPrice = subscription.Price * (1 - (decimal)(highestDiscount / 100 + returnDiscount));
            }
            else
            {
                discountedPrice = subscription.Price * (1 - (decimal)returnDiscount);
            }

            return discountedPrice;
        }
    }
}