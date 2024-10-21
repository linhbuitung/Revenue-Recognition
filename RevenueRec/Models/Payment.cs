using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRec.Models
{
    public class Payment
    {
        [Key]
        public int IdPayment { get; set; }

        public int? IdUpFrontContract { get; set; }

        [ForeignKey("IdUpFrontContract")]
        public UpFrontContract? UpFrontContract { get; set; }

        public int? IdSubscription { get; set; }

        [ForeignKey("IdSubscription")]
        public Subscription? Subscription { get; set; }

        public int IdClient { get; set; }

        [ForeignKey("IdClient")]
        public Client Client { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }
    }
}