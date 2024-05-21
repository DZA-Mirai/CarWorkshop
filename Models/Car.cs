using System.ComponentModel.DataAnnotations;

namespace CarWorkshop.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int RegId { get; set; }
        public string? Image { get; set; }
    }
}
