using RevenueRec.Context;
using RevenueRec.Models;
using RevenueRec.RequestDtos;
using RevenueRec.ResponseDtos;
using RevenueRec.Services.Interfaces;

namespace RevenueRec.Services.Implementations
{
    public class CompanyClientService : ICompanyClientService
    {
        private readonly s28786Context _context;

        public CompanyClientService(s28786Context context)
        {
            _context = context;
        }

        public async Task<CompanyClientResponseDto> AddCompanyClient(CompanyClientRequestDto requestDto)
        {
            CompanyClient companyClient = new CompanyClient
            {
                CompanyName = requestDto.CompanyName,
                KRS = requestDto.KRS,
                Address = requestDto.Address,
                Email = requestDto.Email,
                PhoneNumber = requestDto.PhoneNumber
            };

            _context.CompanyClients.Add(companyClient);

            await _context.SaveChangesAsync();
            return new CompanyClientResponseDto
            {
                IdClient = companyClient.IdClient,
                CompanyName = companyClient.CompanyName,
                KRS = companyClient.KRS,
                Address = companyClient.Address,
                Email = companyClient.Email,
                PhoneNumber = companyClient.PhoneNumber
            };
        }

        public async Task<CompanyClientResponseDto> UpdateCompanyClient(CompanyClientRequestDto requestDto)
        {
            CompanyClient existingClient = _context.CompanyClients
                .FirstOrDefault(c => c.IdClient == requestDto.IdClient);
            if (existingClient != null)
            {
                existingClient.CompanyName = requestDto.CompanyName;
                existingClient.Address = requestDto.Address;
                existingClient.Email = requestDto.Email;
                existingClient.PhoneNumber = requestDto.PhoneNumber;

                // KRS not updated
                await _context.SaveChangesAsync();
                return new CompanyClientResponseDto
                {
                    IdClient = existingClient.IdClient,
                    CompanyName = existingClient.CompanyName,
                    KRS = existingClient.KRS,
                    Address = existingClient.Address,
                    Email = existingClient.Email,
                    PhoneNumber = existingClient.PhoneNumber
                };
            }
            else
            {
                throw new Exception("Client not found");
            }
        }
    }
}