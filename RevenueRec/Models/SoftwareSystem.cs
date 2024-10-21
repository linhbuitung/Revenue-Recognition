using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRec.Models
{
    public class SoftwareSystem
    {
        [Key]
        public int IdSoftwareSystem { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string CurrentVersionInfo { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal YearlyCost { get; set; }

        public int IdCategory { get; set; }

        [ForeignKey("IdCategory")]
        public Category Category { get; set; }

        public ICollection<UpFrontContract> UpFrontContracts { get; set; } = new List<UpFrontContract>();

        public ICollection<SoftwareVersion> SoftwareVersions { get; set; } = new List<SoftwareVersion>();
    }
}