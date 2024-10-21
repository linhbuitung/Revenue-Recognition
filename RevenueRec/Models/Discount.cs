using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRec.Models
{
    public class Discount
    {
        [Key]
        public int IdDiscount { get; set; }

        public string OfferInfo { get; set; }

        public bool IsForUpFront { get; set; } //false = for subscription
        public double Percentage { get; set; }

        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
}