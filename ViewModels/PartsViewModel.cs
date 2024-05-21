using CarWorkshop.Models;

namespace CarWorkshop.ViewModels
{
    public class PartsViewModel
    {
        public int TicketId { get; set; }
        public Part NewPart { get; set; } = new Part();
    }
}
