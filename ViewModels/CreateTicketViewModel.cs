using CarWorkshop.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarWorkshop.ViewModels
{
    public class CreateTicketViewModel
    {
        public int Id { get; set; }
        public Car Car { get; set; }
        public IFormFile? Image { get; set; }
        public int? EmployeeId { get; set; }
        public string Description { get; set; }
        public DateTimeSlot? DateTimeSlots { get; set; }
    }
}
