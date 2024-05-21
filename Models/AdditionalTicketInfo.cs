using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarWorkshop.Models
{
    public class AdditionalTicketInfo
    {
        [Key]
        public int Id { get; set; }
        [NotMapped]
        public double ExpectedCost => Parts?.Sum(p => p.UnitPrice * p.Amount) ?? 0;
        [ForeignKey("Part")]
        public List<int>? PartsId { get; set; }
        public List<Part>? Parts { get; set; }
        public double? PricePaid { get; set; }
    }
}
