using RevenueRec.Models;
using RevenueRec.Context;
using RevenueRec.RequestDtos;
using RevenueRec.ResponseDtos;
using System;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using RevenueRec.Services.Interfaces;

namespace RevenueRec.Services.Implementations
{
    public class IndividualClientService : IIndividualClientService
    {
        private readonly s28786Context _context;

        public IndividualClientService(s28786Context context)
        {
            _context = context;
        }

        public async Task<IndividualClientResponseDto> AddIndividualClient(IndividualClientRequestDto requestDto)
        {
            IndividualClient individualClient = new IndividualClient
            {
                FirstName = requestDto.FirstName,
                LastName = requestDto.LastName,
                Address = requestDto.Address,
                Email = requestDto.Email,
                PhoneNumber = requestDto.PhoneNumber,
                PESEL = requestDto.PESEL
            };

            _context.IndividualClients.Add(individualClient);

            await _context.SaveChangesAsync();
            return new IndividualClientResponseDto
            {
                IdClient = individualClient.IdClient,
                FirstName = individualClient.FirstName,
                LastName = individualClient.LastName,
                Address = individualClient.Address,
                Email = individualClient.Email,
                PhoneNumber = individualClient.PhoneNumber,
                PESEL = individualClient.PESEL
            };
        }

        public async Task<IndividualClientResponseDto> UpdateIndividualClient(IndividualClientRequestDto requestDto)
        {
            IndividualClient existingClient = _context.IndividualClients
                .FirstOrDefault(c => c.IdClient == requestDto.IdClient && !c.IsDeleted);
            if (existingClient != null)
            {
                existingClient.FirstName = requestDto.FirstName;
                existingClient.LastName = requestDto.LastName;
                existingClient.Address = requestDto.Address;
                existingClient.Email = requestDto.Email;
                existingClient.PhoneNumber = requestDto.PhoneNumber;
                // PESEL not updated
                await _context.SaveChangesAsync();
                return new IndividualClientResponseDto
                {
                    IdClient = existingClient.IdClient,
                    FirstName = existingClient.FirstName,
                    LastName = existingClient.LastName,
                    Address = existingClient.Address,
                    Email = existingClient.Email,
                    PhoneNumber = existingClient.PhoneNumber,
                    PESEL = existingClient.PESEL
                };
            }
            else
            {
                throw new Exception("Client not found");
            }
        }

        public async Task SoftDeleteIndividualClient(int deleteClientId)
        {
            IndividualClient client = _context.IndividualClients
                .FirstOrDefault(c => c.IdClient == deleteClientId && !c.IsDeleted);
            if (client != null)
            {
                client.IsDeleted = true;
                client.FirstName = null;
                client.LastName = null;
                client.Address = null;
                client.Email = null;
                client.PhoneNumber = null;
                client.PESEL = null;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Client not found");
            }
        }
    }
}