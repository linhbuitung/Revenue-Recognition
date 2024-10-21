using RevenueRec.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRec.ResponseDtos
{
    public class PaymentResponseDto
    {
        public int IdPayment { get; set; }
        public int? IdUpFrontContract { get; set; }

        public int? IdSubscription { get; set; }

        public int IdClient { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }
    }
}