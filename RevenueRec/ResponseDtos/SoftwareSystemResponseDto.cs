using RevenueRec.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RevenueRec.ResponseDtos
{
    public class SoftwareSystemResponseDto
    {
        public int IdSoftwareSystem { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string CurrentVersionInfo { get; set; }

        public decimal YearlyCost { get; set; }
        public string Category { get; set; }

        public ICollection<SoftwareVersionResponseDto> SoftwareVersions { get; set; } = new List<SoftwareVersionResponseDto>();
    }
}