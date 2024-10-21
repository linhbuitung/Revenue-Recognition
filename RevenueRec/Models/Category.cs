using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRec.Models
{
    public class Category
    {
        [Key]
        public int IdCategory { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public ICollection<SoftwareSystem> SoftwareSystems { get; set; } = new List<SoftwareSystem>();
    }
}