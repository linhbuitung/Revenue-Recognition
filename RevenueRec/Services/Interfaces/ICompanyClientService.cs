using RevenueRec.RequestDtos;
using RevenueRec.ResponseDtos;

namespace RevenueRec.Services.Interfaces
{
    public interface ICompanyClientService
    {
        public Task<CompanyClientResponseDto> AddCompanyClient(CompanyClientRequestDto companyClientRequestDto);

        public Task<CompanyClientResponseDto> UpdateCompanyClient(CompanyClientRequestDto companyClientRequestDto);
    }
}