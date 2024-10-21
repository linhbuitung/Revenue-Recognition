using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using RevenueRec.Context;
using RevenueRec.Controllers;
using RevenueRec.Models;
using RevenueRec.RequestDtos;
using RevenueRec.ResponseDtos;
using RevenueRec.Services.Implementations;

namespace TestRevenue
{
    public class UnitTestForCompanyClientEndpoints
    {
        [Fact]
        public async void AddCompanyClientTest()
        {
            var options = new DbContextOptionsBuilder<s28786Context>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            var context = new s28786Context(options);

            var servcie = new CompanyClientService(context);

            CompanyClientRequestDto company = new CompanyClientRequestDto
            {
                CompanyName = "Company",
                Address = "ul. Kowalska 1",
                Email = "123@gmail.com",
                PhoneNumber = "123456789",
                KRS = "123456789"
            };
            CompanyClientResponseDto responseDto = await servcie.AddCompanyClient(company);
            CompanyClient client = await context.CompanyClients.Where(e => e.IdClient == responseDto.IdClient).FirstOrDefaultAsync();

            Assert.Equal(company.CompanyName, client.CompanyName);
            Assert.Equal(company.Address, client.Address);
            Assert.Equal(company.Email, client.Email);
            Assert.Equal(company.PhoneNumber, client.PhoneNumber);
            Assert.Equal(company.KRS, client.KRS);
        }

        [Fact]
        public async void UpdateCompanyClientTest()
        {
            var options = new DbContextOptionsBuilder<s28786Context>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            var context = new s28786Context(options);

            var servcie = new CompanyClientService(context);

            CompanyClientRequestDto company = new CompanyClientRequestDto
            {
                CompanyName = "Company",
                Address = "ul. Kowalska 1",
                Email = "123@gmail.com",
                PhoneNumber = "123456789",
                KRS = "123456789"
            };
            CompanyClientResponseDto responseDto = await servcie.AddCompanyClient(company);

            var companyUpdate = new CompanyClientRequestDto
            {
                IdClient = responseDto.IdClient,
                CompanyName = "Changed",
                Address = "Changed",
                Email = "Changed",
                PhoneNumber = "Changed"
            };

            CompanyClientResponseDto updatedResponseDto = await servcie.UpdateCompanyClient(companyUpdate);
            CompanyClient companyRes = await context.CompanyClients.Where(e => e.IdClient == responseDto.IdClient).FirstOrDefaultAsync();

            Assert.Equal(companyRes.CompanyName, updatedResponseDto.CompanyName);
            Assert.Equal(companyRes.Address, updatedResponseDto.Address);
            Assert.Equal(companyRes.Email, updatedResponseDto.Email);
            Assert.Equal(companyRes.PhoneNumber, updatedResponseDto.PhoneNumber);
            Assert.Equal(companyRes.KRS, updatedResponseDto.KRS);
        }
    }
}