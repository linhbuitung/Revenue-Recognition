using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRec.Models
{
    public class SoftwareVersion
    {
        [Key]
        public int IdSoftwareVersion { get; set; }

        public string Version { get; set; }
        public string Description { get; set; }

        public DateTime ReleaseDate { get; set; }
        public int IdSoftwareSystem { get; set; }

        [ForeignKey("IdSoftwareSystem")]
        public SoftwareSystem SoftwareSystem { get; set; }

        public ICollection<UpFrontContract> UpFrontContracts { get; set; } = new List<UpFrontContract>();
    }
}