using CarWorkshop.Data;
using CarWorkshop.Interfaces;
using CarWorkshop.Models;
using Microsoft.EntityFrameworkCore;

namespace CarWorkshop.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDBContext _context;

        public EmployeeRepository(ApplicationDBContext context) 
        {
            _context = context;
        }
        public bool Add(Employee employee)
        {
            _context.Add(employee);
            return Save();
        }

        public bool Delete(Employee employee)
        {
            _context.Remove(employee);
            return Save();
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _context.Employees.Include(i => i.Tickets).ThenInclude(t => t.Car).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Employee> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Employees.Include(i => i.Tickets).ThenInclude(t => t.Car).AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Employee employee)
        {
            _context.Update(employee);
            return Save();
        }
    }
}
