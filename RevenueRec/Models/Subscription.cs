using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRec.Models
{
    public class Subscription
    {
        [Key]
        public int IdSubscription { get; set; }

        public int IdClient { get; set; }

        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        public int IdSoftwareSystem { get; set; }

        [ForeignKey("SoftwareSystemId")]
        public SoftwareSystem SoftwareSystem { get; set; }

        /*Name
        Renewal period (monthly, yearly, etc.)
        Price that has to be paid at the beginning of each renewal period
        The renewal period should be at least 1 month and up to 2 years*/

        public string Name { get; set; }
        public RenewalPeriod RenewalPeriod { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        public DateTime StartDate { get; set; }

        //Begining of the next period is count as 10 days max after end date
        public DateTime EndDate { get; set; }

        public bool IsCancelled { get; set; }

        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }

    public enum RenewalPeriod
    {
        Monthly = 1,
        Quarterly = 3,
        Semiannually = 6,
        Yearly = 12,
        TwoYears = 24
    }
}