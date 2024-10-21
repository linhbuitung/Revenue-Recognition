using Microsoft.AspNetCore.Mvc;
using RevenueRec.RequestDtos;
using RevenueRec.ResponseDtos;

namespace RevenueRec.Services.Interfaces
{
    public interface ISubscriptionService
    {
        public Task<SubscriptionResponseDto> AddSubscription(SubscriptionRequestDto subscriptionRequestDto);

        public Task<SubscriptionResponseDto> MakePaymentToSubscription(PaymentForSubscriptionRequestDto payment);
    }
}