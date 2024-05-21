using CarWorkshop.Models;

namespace CarWorkshop.ViewModels
{
    public class EditTicketViewModel
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public Car Car { get; set; }
        public IFormFile Image { get; set; }
        public string? URL { get; set; }
        public int? EmployeeId { get; set; }
        public string Description { get; set; }
    }
}
