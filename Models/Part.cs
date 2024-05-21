using System.ComponentModel.DataAnnotations;

namespace CarWorkshop.Models
{
    public class Part
    {
        [Key]
        public int Id { get; set; }
        public string? PartName { get; set; }
        public double? Amount { get; set; }
        public double? UnitPrice { get; set; }
    }
}
