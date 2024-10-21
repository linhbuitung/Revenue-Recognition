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
    public class UnitTestForUpfronContractEndpoints
    {
        [Fact]
        public async void AddUpFrontContractTest()
        {
            var options = new DbContextOptionsBuilder<s28786Context>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            var context = new s28786Context(options);

            var servcieUpfront = new UpFrontContractService(context);
            var serviceClient = new IndividualClientService(context);
            var serviceSoftware = new SoftwareService(context);
            //add a client
            var client = new IndividualClientRequestDto
            {
                FirstName = "Jan",
                LastName = "Kowalski",
                Address = "ul. Kowalska 1",
                Email = "123@gmail.com",
                PESEL = "12345678901"
            };
            IndividualClientResponseDto resClient = await serviceClient.AddIndividualClient(client);
            //add a category
            Category category = new Category
            {
                Name = "Category"
            };
            context.Categories.Add(category);
            context.SaveChanges();
            //add a software system
            SoftwareSystemRequestDto software = new SoftwareSystemRequestDto
            {
                Name = "Software",
                Description = "Software",
                CurrentVersionInfo = "1.0",
                YearlyCost = 100,
                IdCategory = 1
            };

            await serviceSoftware.CreateSoftwareSystem(software);

            //add a software version
            SoftwareVersionRequestDto version = new SoftwareVersionRequestDto
            {
                Version = "1.0",
                ReleaseDate = DateTime.Now,
                Description = "Init",
                IdSoftwareSystem = 1
            };
            await serviceSoftware.CreateSoftwareVersion(version);

            UpFrontContractCreateRequestDto upfrontContract = new UpFrontContractCreateRequestDto
            {
                PossibleUpdate = "Update",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(10),
                Price = 100,
                IsCancelled = false,
                IsSigned = false,
                AdditionalSupportYears = 1,
                IdClient = resClient.IdClient,
                IdSoftwareSystem = 1,
                IdSoftwareVersion = 1
            };

            UpFrontContractResponseDto responseDto = await servcieUpfront.AddContract(upfrontContract);
            UpFrontContract contract = await context.UpFrontContracts.Where(e => e.IdUpFrontContract == responseDto.IdUpFrontContract).FirstOrDefaultAsync();

            Assert.Equal(upfrontContract.PossibleUpdate, contract.PossibleUpdate);
            Assert.Equal(upfrontContract.StartDate, contract.StartDate);
            Assert.Equal(upfrontContract.EndDate, contract.EndDate);
            Assert.Equal(upfrontContract.Price + 1000 * upfrontContract.AdditionalSupportYears, contract.Price);
            Assert.Equal(upfrontContract.IsCancelled, contract.IsCancelled);
            Assert.Equal(upfrontContract.IsSigned, contract.IsSigned);
            Assert.Equal(upfrontContract.AdditionalSupportYears + 1, contract.SupportYears);
            Assert.Equal(upfrontContract.IdClient, contract.IdClient);
            Assert.Equal(upfrontContract.IdSoftwareSystem, contract.IdSoftwareSystem);
            Assert.Equal(upfrontContract.IdSoftwareVersion, contract.IdSoftwareVersion);
        }

        [Fact]
        public async void MakePaymentToUpFrontContractTest()
        {
            var options = new DbContextOptionsBuilder<s28786Context>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            var context = new s28786Context(options);

            var servcieUpfront = new UpFrontContractService(context);
            var serviceClient = new IndividualClientService(context);
            var serviceSoftware = new SoftwareService(context);
            //add a client
            var client = new IndividualClientRequestDto
            {
                FirstName = "Jan",
                LastName = "Kowalski",
                Address = "ul. Kowalska 1",
                Email = "123@gmail.com",
                PESEL = "12345678901"
            };
            IndividualClientResponseDto resClient = await serviceClient.AddIndividualClient(client);
            //add a category
            Category category = new Category
            {
                Name = "Category"
            };
            context.Categories.Add(category);
            context.SaveChanges();
            //add a software system
            SoftwareSystemRequestDto software = new SoftwareSystemRequestDto
            {
                Name = "Software",
                Description = "Software",
                CurrentVersionInfo = "1.0",
                YearlyCost = 100,
                IdCategory = 1
            };

            await serviceSoftware.CreateSoftwareSystem(software);

            //add a software version
            SoftwareVersionRequestDto version = new SoftwareVersionRequestDto
            {
                Version = "1.0",
                ReleaseDate = DateTime.Now,
                Description = "Init",
                IdSoftwareSystem = 1
            };
            await serviceSoftware.CreateSoftwareVersion(version);

            UpFrontContractCreateRequestDto upfrontContract = new UpFrontContractCreateRequestDto
            {
                PossibleUpdate = "Update",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(10),
                Price = 100,
                IsCancelled = false,
                IsSigned = false,
                AdditionalSupportYears = 1,
                IdClient = resClient.IdClient,
                IdSoftwareSystem = 1,
                IdSoftwareVersion = 1
            };

            UpFrontContractResponseDto responseDto = await servcieUpfront.AddContract(upfrontContract);

            PaymentForUpFrontRequestDto paymentDto = new PaymentForUpFrontRequestDto
            {
                IdClient = resClient.IdClient,
                IdUpFrontContract = responseDto.IdUpFrontContract,
                Amount = responseDto.Price,
                PaymentDate = DateTime.Now.AddDays(1)
            };
            context.SaveChanges();
            UpFrontContractResponseDto responsePaymentDto = await servcieUpfront.MakePaymentToUpFrontContract(paymentDto);
            Assert.Equal(responsePaymentDto.PossibleUpdate, upfrontContract.PossibleUpdate);
            Assert.Equal(responsePaymentDto.StartDate, upfrontContract.StartDate);
            Assert.Equal(responsePaymentDto.EndDate, upfrontContract.EndDate);
            Assert.Equal(responsePaymentDto.Price, upfrontContract.Price + 1000 * upfrontContract.AdditionalSupportYears);
            Assert.Equal(responsePaymentDto.IsCancelled, false);
            Assert.Equal(responsePaymentDto.IsSigned, true);
            Assert.Equal(responsePaymentDto.SupportYears, upfrontContract.AdditionalSupportYears + 1);
            Assert.Equal(responsePaymentDto.IdClient, upfrontContract.IdClient);
            Assert.Equal(responsePaymentDto.IdSoftwareSystem, upfrontContract.IdSoftwareSystem);
            Assert.Equal(responsePaymentDto.IdSoftwareVersion, upfrontContract.IdSoftwareVersion);
        }
    }
}