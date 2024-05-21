using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarWorkshop.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Image { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public double? Salary { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Gender { get; set; }
        public DateTime? WorkingSince { get; set; }
        public bool IsAdmin { get; set; }
        [ForeignKey("Ticket")]
        public List<int>? TicketsId { get; set; }
        public List<Ticket>? Tickets { get; set; }
    }
}
