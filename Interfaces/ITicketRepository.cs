using CarWorkshop.Models;

namespace CarWorkshop.Interfaces
{
    public interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> GetAll();
        Task<Part> GetPartByIdAsync(int id);
        Task<DateTimeSlot> GetSlotByIdAsync(int id);
        Task<Ticket> GetByIdAsync(int id);
        Task<Ticket> GetByIdAsyncNoTracking(int id);
        bool Add(Ticket ticket);
        bool Update(Ticket ticket);
        bool Delete(Ticket ticket);
        bool Save();
        bool DeleteDates(DateTimeSlot slot);
        bool DeletePart(Part part);
    }
}
