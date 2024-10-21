using RevenueRec.ResponseDtos;
using RevenueRec.RequestDtos;

namespace RevenueRec.Services.Interfaces
{
    public interface ISoftwareService
    {
        public Task<SoftwareSystemResponseDto> CreateSoftwareSystemWithInitVersion(SoftwareSystemRequestDto softwareSystemRequestDto, SoftwareVersionRequestDto softwareVersionRequestDto);

        public Task<SoftwareSystemResponseDto> CreateSoftwareSystem(SoftwareSystemRequestDto softwareSystemRequestDto);

        public Task<SoftwareVersionResponseDto> CreateSoftwareVersion(SoftwareVersionRequestDto softwareVersionRequestDto);
    }
}