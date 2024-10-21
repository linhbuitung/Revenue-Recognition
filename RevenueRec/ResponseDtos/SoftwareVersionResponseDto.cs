using RevenueRec.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RevenueRec.ResponseDtos
{
    public class SoftwareVersionResponseDto
    {
        public int IdSoftwareVersion { get; set; }

        public string Version { get; set; }
        public string Description { get; set; }

        public DateTime ReleaseDate { get; set; }
        public int IdSoftwareSystem { get; set; }
    }
}