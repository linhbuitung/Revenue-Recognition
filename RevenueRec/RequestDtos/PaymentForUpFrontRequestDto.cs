using RevenueRec.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRec.RequestDtos
{
    public class PaymentForUpFrontRequestDto
    {
        public int IdUpFrontContract { get; set; }

        public int IdClient { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }
    }
}