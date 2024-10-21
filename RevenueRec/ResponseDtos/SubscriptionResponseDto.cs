using RevenueRec.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RevenueRec.ResponseDtos
{
    public class SubscriptionResponseDto
    {
        [Key]
        public int IdSubscription { get; set; }

        public int IdClient { get; set; }

        public int IdSoftwareSystem { get; set; }

        public string Name { get; set; }
        public RenewalPeriod RenewalPeriod { get; set; }
        public decimal Price { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsCancelled { get; set; }

        public ICollection<PaymentResponseDto> Payments { get; set; } = new List<PaymentResponseDto>();
    }
}