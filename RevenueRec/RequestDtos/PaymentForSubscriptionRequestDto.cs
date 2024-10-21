using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RevenueRec.RequestDtos
{
    public class PaymentForSubscriptionRequestDto
    {
        public int IdSubscription { get; set; }

        public int IdClient { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }
    }
}