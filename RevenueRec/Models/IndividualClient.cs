using System.ComponentModel.DataAnnotations;

namespace RevenueRec.Models
{
    public class IndividualClient : Client
    {
        [MaxLength(100)]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }

        public string? PESEL { get; set; }
        public bool IsDeleted { get; set; }
    }
}