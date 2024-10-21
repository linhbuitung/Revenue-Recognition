using RevenueRec.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RevenueRec.ResponseDtos
{
    public class UpFrontContractResponseDto
    {
        public int IdUpFrontContract { get; set; }

        public string PossibleUpdate { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public decimal Price { get; set; }

        public bool IsCancelled { get; set; }
        public bool IsSigned { get; set; }

        public DateTime ContractStartDate { get; set; }
        public int SupportYears { get; set; }
        public ICollection<PaymentResponseDto> Payments { get; set; }

        public int IdClient { get; set; }

        public int IdSoftwareSystem { get; set; }

        public int IdSoftwareVersion { get; set; }
    }
}