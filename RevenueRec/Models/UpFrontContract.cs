using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRec.Models
{
    public class UpFrontContract
    {
        [Key]
        public int IdUpFrontContract { get; set; }

        public string PossibleUpdate { get; set; } //Possible update for the software system

        //EndDate - StartDate min 3, max 30

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        public bool IsCancelled { get; set; }
        public bool IsSigned { get; set; }

        public DateTime ContractStartDate { get; set; }
        public int SupportYears { get; set; }
        public ICollection<Payment> Payments { get; set; }

        public int IdClient { get; set; }

        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        public int IdSoftwareSystem { get; set; }

        [ForeignKey("SoftwareSystemId")]
        public SoftwareSystem SoftwareSystem { get; set; }

        public int IdSoftwareVersion { get; set; }

        [ForeignKey("SoftwareVersionId")]
        public SoftwareVersion SoftwareVersion { get; set; }
    }
}