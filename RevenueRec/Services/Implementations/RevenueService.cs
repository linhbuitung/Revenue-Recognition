using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using RevenueRec.Context;
using RevenueRec.Models;
using RevenueRec.ResponseDtos;
using RevenueRec.Services.Interfaces;

namespace RevenueRec.Services.Implementations
{
    public class RevenueService : IRevenueService
    {
        private readonly s28786Context _context;
        private static readonly HttpClient client = new HttpClient();
        string appId = "fca_live_vgIw2iMtrKgzdf2on8VbWMhulPLpj2dlgVyDGaMf";

        public RevenueService(s28786Context context)
        {
            _context = context;
        }

        public async Task<RevenueResponseDto> CalculateRevenue(int? idSoftwareSystem, string? currencyCode)
        {
            decimal revenuePLN = 0;
            if (idSoftwareSystem == null)
            {
                //sum all price of signed contract
                revenuePLN += _context.UpFrontContracts.Where(e => e.IsSigned == true).Sum(c => c.Price);

                //sum all payments from every subscription
                revenuePLN += _context.Payments.Where(e => e.IdSubscription != null).Sum(c => c.Amount);
            }
            else
            {
                if (!_context.SoftwareSystems.Any(e => e.IdSoftwareSystem == idSoftwareSystem))
                {
                    throw new Exception("software system not exist");
                }

                revenuePLN += _context.UpFrontContracts.Where(e => e.IsSigned == true && e.IdSoftwareSystem == idSoftwareSystem).Sum(c => c.Price);
                //get all payments that are related to subscription that are related to software system
                revenuePLN += _context.Payments.Where(e => e.IdSubscription != null && _context.Subscriptions.Any(c => c.IdSoftwareSystem == idSoftwareSystem)).Sum(c => c.Amount);
            }

            string revenueExchanged = revenuePLN.ToString();
            decimal resExchanged = revenuePLN;
            if (currencyCode != null)
            {
                resExchanged = await ExchangeFromPLN(currencyCode, revenuePLN);
                revenueExchanged = currencyCode.ToUpper();
                return new RevenueResponseDto
                {
                    Revenue = resExchanged,
                    Currency = revenueExchanged
                };
            }

            return new RevenueResponseDto
            {
                Revenue = revenuePLN,
                Currency = "PLN"
            };
        }

        public async Task<RevenueResponseDto> PredictRevenue(DateTime timePoint, int? idSoftwareSystem, string? currencyCode)
        {
            decimal revenuePLN = 0;
            if (idSoftwareSystem == null)
            {
                //sum all price of non-cancelled
                revenuePLN += _context.UpFrontContracts.Where(e => e.IsCancelled == false).Sum(c => c.Price);

                //get list of all subscriptions
                List<Subscription> subscriptions = await _context.Subscriptions.ToListAsync();
                foreach (Subscription subscription in subscriptions)
                {
                    //get all payments that are related to subscription

                    revenuePLN += _context.Payments.Where(e => e.IdSubscription == subscription.IdSubscription).Sum(c => c.Amount);
                    //predict next period = sum of all to be paid payments
                    decimal sum = 0;
                    if (timePoint > subscription.EndDate)
                    {
                        DateTime temp = subscription.EndDate;
                        do
                        {
                            sum += await GetPeriodPrice(subscription.IdSubscription, false);
                            temp = temp.AddMonths((int)subscription.RenewalPeriod);
                        } while (temp < timePoint);
                    }

                    revenuePLN += sum;
                }
            }
            else
            {
                if (!_context.SoftwareSystems.Any(e => e.IdSoftwareSystem == idSoftwareSystem))
                {
                    throw new Exception("software system not exist");
                }
                revenuePLN += _context.UpFrontContracts.Where(e => e.IsCancelled == false && e.IdSoftwareSystem == idSoftwareSystem).Sum(c => c.Price);

                List<Subscription> subscriptions = await _context.Subscriptions.Where(e => e.IdSoftwareSystem == idSoftwareSystem).ToListAsync();
                foreach (Subscription subscription in subscriptions)
                {
                    //get all payments that are related to subscription that are related to software system

                    revenuePLN += _context.Payments.Where(e => e.IdSubscription == subscription.IdSubscription).Sum(c => c.Amount);
                    //predict next period

                    decimal sum = 0;
                    DateTime temp = subscription.EndDate;
                    do
                    {
                        sum += await GetPeriodPrice(subscription.IdSubscription, false);
                        temp = temp.AddMonths((int)subscription.RenewalPeriod);
                    } while (temp < timePoint);

                    revenuePLN += sum;
                }
            }
            string revenueExchanged = revenuePLN.ToString();
            decimal resExchanged = revenuePLN;
            if (currencyCode != null)
            {
                resExchanged = await ExchangeFromPLN(currencyCode, revenuePLN);
                revenueExchanged = currencyCode.ToUpper();
                return new RevenueResponseDto
                {
                    Revenue = resExchanged,
                    Currency = revenueExchanged
                };
            }

            return new RevenueResponseDto
            {
                Revenue = revenuePLN,
                Currency = "PLN"
            };
        }

        public async Task<decimal> ExchangeFromPLN(string targetLang, decimal currency)
        {
            string url = "https://api.freecurrencyapi.com/v1/latest?apikey=" + appId + "&currencies=" + targetLang.ToUpper() + "&base_currency=PLN";

            HttpResponseMessage response = await client.GetAsync(url);
            //error if not success 200 ok
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("exchange  api response error");
            }
            string responseBody = await response.Content.ReadAsStringAsync();
            double rate = ParseAndReturnRate(responseBody, targetLang.ToUpper());

            currency = currency * (decimal)rate;
            return currency;
        }

        private static double ParseAndReturnRate(string jsonResponse, string ReturnLanguage)
        {
            JObject data = JObject.Parse(jsonResponse);

            /*get rate as double from example response:
                 {
                    "data":
                        {
                            "EUR": 0.2311928853
                        }
                }*/
            double rate = data["data"][ReturnLanguage].Value<double>();
            return rate;
        }

        private async Task<decimal> GetPeriodPrice(int id, bool isFirst)
        {
            Subscription subscription = await _context.Subscriptions.Where(s => s.IdSubscription == id).FirstOrDefaultAsync();

            double highestDiscount = 0;
            if (_context.Discounts.Any(e => e.IsForUpFront == true))
            {
                highestDiscount = _context.Discounts.Where(d => d.IsForUpFront).Max(d => d.Percentage);
            }
            double returnDiscount = 0;
            if (await IsReturningClient(subscription.IdClient))
            {
                returnDiscount = 0.05;
            }
            decimal discountedPrice = 0;
            if (isFirst)
            {
                discountedPrice = subscription.Price * (1 - (decimal)(highestDiscount / 100 + returnDiscount));
            }
            else
            {
                discountedPrice = subscription.Price * (1 - (decimal)returnDiscount);
            }

            return discountedPrice;
        }

        private async Task<bool> IsReturningClient(int clientId)
        {
            var hasContract = await _context.UpFrontContracts.AnyAsync(c => c.IdClient == clientId && c.IsSigned);
            var hasSubscription = await _context.Subscriptions.AnyAsync(s => s.IdClient == clientId);

            return hasSubscription || hasContract;
        }
    }
}