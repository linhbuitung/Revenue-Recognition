namespace RevenueRec.RequestDtos
{
    public class SoftwareSystemRequestDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string CurrentVersionInfo { get; set; }

        public decimal YearlyCost { get; set; }
        public int IdCategory { get; set; }
    }
}