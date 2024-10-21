namespace RevenueRec.RequestDtos
{
    public class SoftwareVersionRequestDto
    {
        public string Version { get; set; }
        public string Description { get; set; }

        public DateTime ReleaseDate { get; set; }
        public int IdSoftwareSystem { get; set; }
    }
}