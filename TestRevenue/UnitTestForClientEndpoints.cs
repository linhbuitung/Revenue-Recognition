using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using RevenueRec.Context;
using RevenueRec.Controllers;
using RevenueRec.Models;
using RevenueRec.RequestDtos;
using RevenueRec.ResponseDtos;
using RevenueRec.Services.Implementations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TestRevenue
{
    public class UnitTestForClientEndpoints
    {
        [Fact]
        public async void AddIndividualClientTest()
        {
            var options = new DbContextOptionsBuilder<s28786Context>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            var context = new s28786Context(options);

            var servcie = new IndividualClientService(context);

            var client = new IndividualClientRequestDto
            {
                FirstName = "Jan",
                LastName = "Kowalski",
                Address = "ul. Kowalska 1",
                Email = "123@gmail.com",
                PESEL = "12345678901"
            };
            IndividualClientResponseDto resClient = await servcie.AddIndividualClient(client);

            IndividualClient res = await context.IndividualClients.Where(e => e.IdClient == resClient.IdClient).FirstOrDefaultAsync();

            Assert.Equal(client.FirstName, res.FirstName);
            Assert.Equal(client.LastName, res.LastName);
            Assert.Equal(client.Address, res.Address);
            Assert.Equal(client.Email, res.Email);
            Assert.Equal(client.PESEL, res.PESEL);
        }

        [Fact]
        public async void UpdateIndividualClientTest()
        {
            var options = new DbContextOptionsBuilder<s28786Context>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            var context = new s28786Context(options);

            var servcie = new IndividualClientService(context);

            var client = new IndividualClientRequestDto
            {
                FirstName = "Jan",
                LastName = "Kowalski",
                Address = "ul. Kowalska 1",
                Email = "123@gmail.com",
                PESEL = "12345678901"
            };
            IndividualClientResponseDto resClient = await servcie.AddIndividualClient(client);

            var clientUpdate = new IndividualClientRequestDto
            {
                IdClient = resClient.IdClient,
                FirstName = "Changed",
                LastName = "Changed",
                Address = "Changed",
                Email = "Changed"
            };
            IndividualClientResponseDto resClientUpdate = await servcie.UpdateIndividualClient(clientUpdate);
            IndividualClient indiRes = await context.IndividualClients.Where(e => e.IdClient == resClient.IdClient).FirstOrDefaultAsync();

            Assert.Equal(indiRes.FirstName, resClientUpdate.FirstName);
            Assert.Equal(indiRes.LastName, resClientUpdate.LastName);
            Assert.Equal(indiRes.Address, resClientUpdate.Address);
            Assert.Equal(indiRes.Email, resClientUpdate.Email);
            Assert.Equal(indiRes.PESEL, resClientUpdate.PESEL);
        }

        [Fact]
        public async void SoftDeleteIndividualClientTest()
        {
            var options = new DbContextOptionsBuilder<s28786Context>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            var context = new s28786Context(options);

            var servcie = new IndividualClientService(context);

            var client = new IndividualClientRequestDto
            {
                FirstName = "Jan",
                LastName = "Kowalski",
                Address = "ul. Kowalska 1",
                Email = "123@gmail.com",
                PESEL = "12345678901"
            };
            IndividualClientResponseDto resClient = await servcie.AddIndividualClient(client);

            servcie.SoftDeleteIndividualClient(resClient.IdClient);

            IndividualClient resDeletedClient = await context.IndividualClients.Where(e => e.IdClient == resClient.IdClient).FirstOrDefaultAsync();

            Assert.True(resDeletedClient.IsDeleted);
            Assert.Equal(null, resDeletedClient.FirstName);
            Assert.Equal(null, resDeletedClient.LastName);
            Assert.Equal(null, resDeletedClient.Address);
            Assert.Equal(null, resDeletedClient.Email);
            Assert.Equal(null, resDeletedClient.PESEL);
        }
    }
}