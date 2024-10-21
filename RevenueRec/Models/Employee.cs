using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRec.Models
{
    public class Employee
    {
        [Key]
        public int IdEmployee { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
        public string Role { get; set; }
        public string Salt { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExp { get; set; }
    }
}