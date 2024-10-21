namespace RevenueRec.ResponseDtos
{
    public class CompanyClientResponseDto
    {
        public int IdClient { get; set; }

        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CompanyName { get; set; }

        public string KRS { get; set; }
    }
}