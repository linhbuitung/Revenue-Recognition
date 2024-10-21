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
    public class UnitTestForRevenueEndpoints
    {
        [Fact]
        public async void TestGetRevenue()
        {
            var options = new DbContextOptionsBuilder<s28786Context>()
               .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
               .Options;

            var context = new s28786Context(options);

            var servcieUpfront = new UpFrontContractService(context);
            var serviceSubscription = new SubscriptionService(context);
            var serviceInidividualClient = new IndividualClientService(context);
            var serviceCompanyClient = new CompanyClientService(context);
            var serviceRevenue = new RevenueService(context);
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

            var client2 = new IndividualClientRequestDto
            {
                FirstName = "Bui",
                LastName = "Minh Khanh",
                Address = "Hanoi",
                Email = "789@gmail.com",
                PESEL = "9876543264"
            };
            IndividualClientResponseDto resClient = await serviceInidividualClient.AddIndividualClient(client);
            resClient = await serviceInidividualClient.AddIndividualClient(client2);

            var clientCompany = new CompanyClientRequestDto
            {
                CompanyName = "Company",
                Address = "ul. Kowalska 1",
                Email = "789@gmail.com",
                PhoneNumber = "123456789",
                KRS = "123456789"
            };
            CompanyClientResponseDto resComp = await serviceCompanyClient.AddCompanyClient(clientCompany);
            //add a category
            Category category = new Category
            {
                Name = "CategoryOne"
            };
            context.Categories.Add(category);
            category = new Category
            {
                Name = "CategorTwo"
            };
            context.Categories.Add(category);

            context.SaveChanges();
            //add a software system
            /*SoftwareSystem software = new SoftwareSystem
            {
                Name = "SoftwareOne",
                Description = "Software",
                CurrentVersionInfo = "1.0",
                YearlyCost = 100,
                IdCategory = 1
            };*/

            SoftwareSystemRequestDto software = new SoftwareSystemRequestDto
            {
                Name = "SoftwareOne",
                Description = "Software",
                CurrentVersionInfo = "1.0",
                YearlyCost = 100,
                IdCategory = 1
            };

            await serviceSoftware.CreateSoftwareSystem(software);

            software = new SoftwareSystemRequestDto
            {
                Name = "SoftwareTwo",
                Description = "Software",
                CurrentVersionInfo = "1.4",
                YearlyCost = 200,
                IdCategory = 2
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

            version = new SoftwareVersionRequestDto
            {
                Version = "2.0",
                ReleaseDate = DateTime.Now.AddDays(2),
                Description = "Init2",
                IdSoftwareSystem = 1
            };
            await serviceSoftware.CreateSoftwareVersion(version);

            version = new SoftwareVersionRequestDto
            {
                Version = "1.3",
                ReleaseDate = DateTime.Now,
                Description = "Init",
                IdSoftwareSystem = 2
            };
            await serviceSoftware.CreateSoftwareVersion(version);
            //print out this version

            UpFrontContractCreateRequestDto upfrontContract = new UpFrontContractCreateRequestDto
            {
                PossibleUpdate = "Update",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(10),
                Price = 100,
                IsCancelled = false,
                IsSigned = false,
                AdditionalSupportYears = 1,
                IdClient = 1,
                IdSoftwareSystem = 2,
                IdSoftwareVersion = 3
            };
            UpFrontContractResponseDto responseDto = await servcieUpfront.AddContract(upfrontContract);
            upfrontContract = new UpFrontContractCreateRequestDto
            {
                PossibleUpdate = "Update2",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(10),
                Price = 200,
                IsCancelled = false,
                IsSigned = false,
                AdditionalSupportYears = 2,
                IdClient = 2,
                IdSoftwareSystem = 1,
                IdSoftwareVersion = 1
            };
            responseDto = await servcieUpfront.AddContract(upfrontContract);

            SubscriptionRequestDto subscription = new SubscriptionRequestDto
            {
                IdClient = 3,
                IdSoftwareSystem = 1,
                Name = "Subscription",
                RenewalPeriod = RenewalPeriod.Monthly,
                Price = 100,
            };

            SubscriptionResponseDto resSub = await serviceSubscription.AddSubscription(subscription);

            RevenueResponseDto revenue = await serviceRevenue.CalculateRevenue(null, null);

            //because upfront contracts are not paid yet
            Assert.Equal(100, revenue.Revenue);
            Assert.Equal("PLN", revenue.Currency);

            RevenueResponseDto predict = await serviceRevenue.PredictRevenue(DateTime.Now.AddDays(1), null, null);
            Assert.Equal(3400, predict.Revenue);
            Assert.Equal("PLN", revenue.Currency);
        }
    }
}