using RevenueRec.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RevenueRec.RequestDtos
{
    public class UpFrontContractCreateRequestDto
    {
        public string PossibleUpdate { get; set; }

        //EndDate - StartDate min 3, max 30

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        public bool IsCancelled { get; set; }
        public bool IsSigned { get; set; }

        public int AdditionalSupportYears { get; set; }

        public int IdClient { get; set; }

        public int IdSoftwareSystem { get; set; }

        public int IdSoftwareVersion { get; set; }
    }
}