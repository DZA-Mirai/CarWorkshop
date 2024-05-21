//using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarWorkshop.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Car")]
        public int CarId { get; set; }
        public Car Car { get; set; }
        [ForeignKey("Employee")]
        public int? EmployeeId { get; set; }
        public Employee? Employee{ get; set; }
        public string Description { get; set; }
        public bool IsDone { get; set; }    // true means car is ready to pick up
        public bool IsClosed { get; set; }  // true means ticket is archived
        [ForeignKey("DateTimeSlot")]
        public List<int>? DateTimeId { get; set; }
        public List<DateTimeSlot>? DateTimeSlots { get; set; }
        [ForeignKey("AdditionalTicketInfo")]
        public int? AdditionalTicketInfoId { get; set; }
        public AdditionalTicketInfo? AdditionalTicketInfo { get; set; }
    }
}
