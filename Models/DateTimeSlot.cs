using System.ComponentModel.DataAnnotations;

namespace CarWorkshop.Models
{
    public class DateTimeSlot
    {
        [Key]
        public int Id { get; set; }
        public DateTime? From { get; set; }
        public DateTime? Till { get; set; }
    }
}
