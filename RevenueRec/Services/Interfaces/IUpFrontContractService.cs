using Microsoft.AspNetCore.Mvc;
using RevenueRec.RequestDtos;
using RevenueRec.ResponseDtos;

namespace RevenueRec.Services.Interfaces
{
    public interface IUpFrontContractService
    {
        public Task<UpFrontContractResponseDto> AddContract(UpFrontContractCreateRequestDto upFrontContractRequestDto);

        public Task<UpFrontContractResponseDto> MakePaymentToUpFrontContract(PaymentForUpFrontRequestDto payment);

        public Task DeleteContract(int id);
    }
}