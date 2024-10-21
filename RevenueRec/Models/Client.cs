using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRec.Models
{
    public abstract class Client
    {
        [Key]
        public int IdClient { get; set; }

        public string? Address { get; set; }
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        public ICollection<UpFrontContract> UpFrontContracts { get; set; } = new List<UpFrontContract>();

        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}