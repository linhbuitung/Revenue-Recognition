using RevenueRec.RequestDtos;
using RevenueRec.ResponseDtos;

namespace RevenueRec.Services.Interfaces
{
    public interface IIndividualClientService
    {
        public Task<IndividualClientResponseDto> AddIndividualClient(IndividualClientRequestDto individualClientRequestDto);

        public Task<IndividualClientResponseDto> UpdateIndividualClient(IndividualClientRequestDto individualClientRequestDto);

        public Task SoftDeleteIndividualClient(int deleteClientId);
    }
}