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
    public class UnitTestForSubscriptionEnpoints
    {
        [Fact]
        public async void AddSubscriptionTest()
        {
            var options = new DbContextOptionsBuilder<s28786Context>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            var context = new s28786Context(options);

            var servcieSub = new SubscriptionService(context);
            var serviceClient = new IndividualClientService(context);
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
            //add a software system
            SoftwareSystem software = new SoftwareSystem
            {
                Name = "Software",
                Description = "Software",
                CurrentVersionInfo = "1.0",
                YearlyCost = 100,
                IdCategory = 1
            };

            context.SoftwareSystems.Add(software);
            /*public int IdClient { get; set; }

        public int IdSoftwareSystem { get; set; }

        public string Name { get; set; }
        public RenewalPeriod RenewalPeriod { get; set; }
        public decimal Price { get; set; }

        public DateTime StartDate { get; set; }*/
            SubscriptionRequestDto subscription = new SubscriptionRequestDto
            {
                IdClient = 1,
                IdSoftwareSystem = 1,
                Name = "Subscription",
                RenewalPeriod = RenewalPeriod.Monthly,
                Price = 100,
            };

            SubscriptionResponseDto resSub = await servcieSub.AddSubscription(subscription);

            //get first payment
            Payment payment = await context.Payments.Where(e => e.IdSubscription == resSub.IdSubscription).FirstOrDefaultAsync();
            Assert.Equal(subscription.Price, payment.Amount);
            Assert.Equal(subscription.IdClient, payment.IdClient);
            Assert.Equal(resSub.IdSubscription, payment.IdSubscription);

            Assert.Equal(resSub.Name, subscription.Name);
            Assert.Equal(resSub.RenewalPeriod, subscription.RenewalPeriod);
            Assert.Equal(resSub.Price, subscription.Price);
        }

        [Fact]
        public async void PaymentToSubscriptionTest()
        {
            var options = new DbContextOptionsBuilder<s28786Context>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;

            var context = new s28786Context(options);

            var servcieSub = new SubscriptionService(context);
            var serviceClient = new IndividualClientService(context);
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
            //add a software system
            SoftwareSystem software = new SoftwareSystem
            {
                Name = "Software",
                Description = "Software",
                CurrentVersionInfo = "1.0",
                YearlyCost = 100,
                IdCategory = 1
            };

            context.SoftwareSystems.Add(software);
            /*public int IdClient { get; set; }

        public int IdSoftwareSystem { get; set; }

        public string Name { get; set; }
        public RenewalPeriod RenewalPeriod { get; set; }
        public decimal Price { get; set; }

        public DateTime StartDate { get; set; }*/
            SubscriptionRequestDto subscription = new SubscriptionRequestDto
            {
                IdClient = 1,
                IdSoftwareSystem = 1,
                Name = "Subscription",
                RenewalPeriod = RenewalPeriod.Monthly,
                Price = 100,
            };

            SubscriptionResponseDto resSub = await servcieSub.AddSubscription(subscription);

            //Subscription test = context.Subscriptions.Where(e => e.IdSubscription == resSub.IdSubscription).FirstOrDefault();
            PaymentForSubscriptionRequestDto payment = new PaymentForSubscriptionRequestDto
            {
                IdClient = 1,
                IdSubscription = resSub.IdSubscription,
                Amount = 100,
                PaymentDate = resSub.EndDate.AddDays(1)
            };

            SubscriptionResponseDto resSubPayment = await servcieSub.MakePaymentToSubscription(payment);

            Payment test = context.Payments.Where(e => e.IdSubscription == resSub.IdSubscription && e.IdPayment == 1).FirstOrDefault();
            Assert.Equal(payment.Amount, test.Amount);
            Assert.Equal(resSub.EndDate.AddMonths((int)resSub.RenewalPeriod), resSubPayment.EndDate);
        }
    }
}