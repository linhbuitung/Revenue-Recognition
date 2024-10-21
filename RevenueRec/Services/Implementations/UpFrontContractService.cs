using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using RevenueRec.Context;
using RevenueRec.Models;
using RevenueRec.RequestDtos;
using RevenueRec.ResponseDtos;
using RevenueRec.Services.Interfaces;
using System.Diagnostics.Contracts;
using System.Net.Sockets;
using System.Security.Policy;

namespace RevenueRec.Services.Implementations
{
    public class UpFrontContractService : IUpFrontContractService
    {
        private readonly s28786Context _context;

        public UpFrontContractService(s28786Context context)
        {
            _context = context;
        }

        public async Task<UpFrontContractResponseDto> AddContract(UpFrontContractCreateRequestDto upFrontContractRequestDto)
        {
            //The time range should be at least 3 daysand at most 30 days.The contract has to be paid for by the client within this time range.Otherwise contract is cancelled
            if (upFrontContractRequestDto.EndDate.Subtract(upFrontContractRequestDto.StartDate).Days < 3 || upFrontContractRequestDto.EndDate.Subtract(upFrontContractRequestDto.StartDate).Days > 30)
            {
                throw new Exception("The time range should be at least 3 days and at most 30 days.");
            }
            if (await IsSoftwareAlreadyBeingUsed(upFrontContractRequestDto.IdSoftwareSystem, upFrontContractRequestDto.IdClient))
            {
                throw new Exception("The software is already being used by this client.");
            }

            //check if software version belongs to software system
            if (!_context.SoftwareVersions.Any(e => e.IdSoftwareVersion == upFrontContractRequestDto.IdSoftwareVersion && e.IdSoftwareSystem == upFrontContractRequestDto.IdSoftwareSystem))
            {
                throw new Exception("Software version does not belong to software system.");
            }
            var price = await CalculateDiscountedPriceForUpFront(upFrontContractRequestDto);
            UpFrontContract contract = new UpFrontContract
            {
                PossibleUpdate = upFrontContractRequestDto.PossibleUpdate,
                StartDate = upFrontContractRequestDto.StartDate,
                EndDate = upFrontContractRequestDto.EndDate,
                ContractStartDate = DateTime.Now,
                IsSigned = false,
                IsCancelled = false,
                Price = price,
                SupportYears = upFrontContractRequestDto.AdditionalSupportYears + 1,
                IdClient = upFrontContractRequestDto.IdClient,
                IdSoftwareSystem = upFrontContractRequestDto.IdSoftwareSystem,
                IdSoftwareVersion = upFrontContractRequestDto.IdSoftwareVersion
            };

            _context.UpFrontContracts.Add(contract);
            await _context.SaveChangesAsync();

            return new UpFrontContractResponseDto
            {
                IdUpFrontContract = contract.IdUpFrontContract,
                PossibleUpdate = contract.PossibleUpdate,
                StartDate = contract.StartDate,
                EndDate = contract.EndDate,
                ContractStartDate = contract.ContractStartDate,
                IsSigned = contract.IsSigned,
                IsCancelled = contract.IsCancelled,
                Price = contract.Price,
                SupportYears = contract.SupportYears,
                IdClient = contract.IdClient,
                IdSoftwareSystem = contract.IdSoftwareSystem,
                IdSoftwareVersion = contract.IdSoftwareVersion
            };
        }

        public async Task<UpFrontContractResponseDto> MakePaymentToUpFrontContract(PaymentForUpFrontRequestDto paymentDto)
        {
            decimal currentTotalAmount = await _context.Payments.Where(p => p.IdUpFrontContract == paymentDto.IdUpFrontContract).SumAsync(p => p.Amount);
            if (!_context.UpFrontContracts.Any(e => e.IdClient == paymentDto.IdClient && e.IdUpFrontContract == paymentDto.IdUpFrontContract))
            {
                throw new Exception("Contract does not exits for this client");
            }
            UpFrontContract upFrontContract = await _context.UpFrontContracts.FindAsync(paymentDto.IdUpFrontContract);

            if (upFrontContract.IsCancelled)
            {
                throw new Exception("The contract is cancelled.");
            }
            if (upFrontContract.IsSigned)
            {
                throw new Exception("The contract is already paid.");
            }

            if (paymentDto.PaymentDate > upFrontContract.EndDate)
            {
                //update contract to be cancelled
                upFrontContract.IsCancelled = true;
                await _context.SaveChangesAsync();
                //delete all payments related to contract
                var payments = _context.Payments.Where(p => p.IdUpFrontContract == paymentDto.IdUpFrontContract);
                _context.Payments.RemoveRange(payments);
                await _context.SaveChangesAsync();
                throw new Exception("The payment date is after the contract end date.");
            }
            if (currentTotalAmount + paymentDto.Amount > upFrontContract.Price)
            {
                throw new Exception("The payment amount exceeds the total price of the contract.");
            }
            if (currentTotalAmount + paymentDto.Amount == upFrontContract.Price)
            {
                upFrontContract.IsSigned = true;
            }

            Payment payment = new Payment
            {
                Amount = paymentDto.Amount,
                PaymentDate = paymentDto.PaymentDate,
                IdUpFrontContract = paymentDto.IdUpFrontContract,
                IdClient = paymentDto.IdClient
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return new UpFrontContractResponseDto
            {
                IdUpFrontContract = upFrontContract.IdUpFrontContract,
                PossibleUpdate = upFrontContract.PossibleUpdate,
                StartDate = upFrontContract.StartDate,
                EndDate = upFrontContract.EndDate,
                ContractStartDate = upFrontContract.ContractStartDate,
                IsSigned = upFrontContract.IsSigned,
                IsCancelled = upFrontContract.IsCancelled,
                Price = upFrontContract.Price,
                SupportYears = upFrontContract.SupportYears,
                IdClient = upFrontContract.IdClient,
                IdSoftwareSystem = upFrontContract.IdSoftwareSystem,
                IdSoftwareVersion = upFrontContract.IdSoftwareVersion
            };
        }

        public async Task DeleteContract(int id)
        {
            UpFrontContract contract = await _context.UpFrontContracts.FindAsync(id);
            if (contract != null)
            {
                _context.UpFrontContracts.Remove(contract);
                await _context.SaveChangesAsync();
                return;
            }
            else
            {
                throw new Exception("Contract not found");
            }
        }

        public async Task<decimal> CalculateDiscountedPriceForUpFront(UpFrontContractCreateRequestDto upFrontContractRequestDto)
        {
            //get yearlycost for the software system that the client wants to buy
            decimal yearlyCost = await _context.SoftwareSystems.Where(s => s.IdSoftwareSystem == upFrontContractRequestDto.IdSoftwareSystem).Select(s => s.YearlyCost).FirstOrDefaultAsync();

            //check if theres any discount
            double highestDiscount = 0;
            if (_context.Discounts.Any(e => e.IsForUpFront == true))
            {
                highestDiscount = _context.Discounts.Where(d => d.IsForUpFront).Max(d => d.Percentage);
            }

            double returnDiscount = 0;
            if (await IsReturningClient(upFrontContractRequestDto.IdClient, null, null))
            {
                returnDiscount = 0.05;
            }
            var discountedPrice = (yearlyCost + upFrontContractRequestDto.AdditionalSupportYears * 1000) * (1 - (decimal)(highestDiscount / 100 + returnDiscount));

            return discountedPrice;
        }

        private async Task<bool> IsReturningClient(int clientId, int? contractId, int? subscriptionId)
        {
            var hasContract = false;
            if (contractId != null)
            {
                hasContract = await _context.UpFrontContracts.AnyAsync(c => c.IdClient == clientId && c.IsSigned && c.IdUpFrontContract != contractId);
            }
            else
            {
                hasContract = await _context.UpFrontContracts.AnyAsync(c => c.IdClient == clientId && c.IsSigned);
            }
            var hasSubscription = false;
            if (subscriptionId != null)
            {
                hasSubscription = await _context.Subscriptions.AnyAsync(s => s.IdClient == clientId && s.IdSubscription != subscriptionId);
            }
            else
            {
                hasSubscription = await _context.Subscriptions.AnyAsync(s => s.IdClient == clientId);
            }
            return hasSubscription || hasContract;
        }

        private async Task<bool> IsSoftwareAlreadyBeingUsed(int softwareId, int clientId)
        {
            bool res = await _context.UpFrontContracts.AnyAsync(e => e.IdSoftwareSystem == softwareId && e.IdClient == clientId && e.IsCancelled == false)
                || await _context.Subscriptions.AnyAsync(e => e.IdSoftwareSystem == softwareId && e.IdClient == clientId && e.IsCancelled == false);
            return res;
        }
    }
}