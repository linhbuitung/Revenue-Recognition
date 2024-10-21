using RevenueRec.ResponseDtos;

namespace RevenueRec.Services.Interfaces
{
    public interface IRevenueService
    {
        public Task<RevenueResponseDto> CalculateRevenue(int? idSoftwareSystem, string? currencyCode);

        public Task<RevenueResponseDto> PredictRevenue(DateTime timePoint, int? idSoftwareSystem, string? currencyCode);
    }
}