using CarWorkshop.Models;

namespace CarWorkshop.ViewModels
{
    public class TicketVIewModel
    {
        public Ticket Ticket { get; set; }
        public DateTimeSlot NewDateTimeSlot { get; set; } = new DateTimeSlot();
    }
}
