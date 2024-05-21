using CarWorkshop.Models;

namespace CarWorkshop.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAll();
        Task<Employee> GetByIdAsync(int id);
        Task<Employee> GetByIdAsyncNoTracking(int id);
        bool Add(Employee employee);
        bool Update(Employee employee);
        bool Delete(Employee employee);
        bool Save();
    }
}
