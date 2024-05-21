using CarWorkshop.Data;
using CarWorkshop.Models;

namespace CarWorkshop.Helpers
{
    public class AuthorizationManager
    {
        private readonly ApplicationDBContext _context;
        public int id;

        public AuthorizationManager(ApplicationDBContext context)
        {
            _context = context;
            id = -1;
        }
        public Employee ValidateEmployee(string login, string password)
        {
            Employee employee = _context.Employees.FirstOrDefault(u => u.Login == login && u.Password == password);
            if (employee != null)
            {
                id = employee.Id;
            }
            return employee;
        }

        public bool GetEmployeeRole(int? id)
        {
            if (id == null) return false;
            var user = _context.Employees.FirstOrDefault(u => u.Id == id);
            if (user == null) return false;
            return user.IsAdmin;
        }
    }
}
