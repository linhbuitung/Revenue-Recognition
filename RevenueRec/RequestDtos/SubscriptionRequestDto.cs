using RevenueRec.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRec.RequestDtos
{
    public class SubscriptionRequestDto
    {
        public int IdClient { get; set; }

        public int IdSoftwareSystem { get; set; }

        public string Name { get; set; }
        public RenewalPeriod RenewalPeriod { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
    }
}