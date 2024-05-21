using CarWorkshop.Helpers;
using CarWorkshop.Interfaces;
using CarWorkshop.Models;
using CarWorkshop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CarWorkshop.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPhotoService _photoService;
        private readonly AuthorizationManager _currentUser;

        public EmployeeController(IEmployeeRepository employeeRepository, IPhotoService photoService, AuthorizationManager currentUser)
        {
            _employeeRepository = employeeRepository;
            _photoService = photoService;
            _currentUser = currentUser;
        }
        public async Task<IActionResult> Index()
        {
            int? currentId = HttpContext.Session.GetInt32("ID");
            if (currentId == null) return RedirectToAction("Index", "Home");
            if (!_currentUser.GetEmployeeRole(currentId)) return RedirectToAction("AccessDenied", "Home");
            IEnumerable<Employee> employees = await _employeeRepository.GetAll();
            return View(employees);
        }
        
        public async Task<IActionResult> Detail(int id)
        {
            Employee employee = await _employeeRepository.GetByIdAsync(id);
            return View(employee);
        }

        public IActionResult Create()
        {
            int? currentId = HttpContext.Session.GetInt32("ID");
            if (currentId == null) return RedirectToAction("Index", "Home");
            if (!_currentUser.GetEmployeeRole(currentId)) return RedirectToAction("AccessDenied", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(employeeVM.Image);
                var employee = new Employee
                {
                    Name = employeeVM.Name,
                    Surname = employeeVM.Surname,
                    Image = result.Url.ToString(),
                    Login = employeeVM.Login,
                    Password = employeeVM.Password,
                    Salary = employeeVM.Salary,
                    BirthDate = employeeVM.BirthDate,
                    Gender = employeeVM.Gender,
                    WorkingSince = employeeVM.WorkingSince,
                    IsAdmin = employeeVM.IsAdmin
                };
                _employeeRepository.Add(employee);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo Upload Failed");
            }
            
            return View(employeeVM);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return View("Error");
            var employeeVM = new EditEmployeeViewModel
            {
                Name = employee.Name,
                Surname = employee.Surname,
                URL = employee.Image,
                Login = employee.Login,
                Password = employee.Password,
                Salary = employee.Salary,
                BirthDate = employee.BirthDate,
                Gender = employee.Gender,
                WorkingSince = employee.WorkingSince,
                IsAdmin = employee.IsAdmin
            };
            return View(employeeVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditEmployeeViewModel employeeVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit");
                return View("Edit", employeeVM);
            }

            var userEmployee = await _employeeRepository.GetByIdAsyncNoTracking(id);

            if (userEmployee != null)
            {
                if(userEmployee.Image != null)
                {
                    try
                    {
                        await _photoService.DeletePhotoAsync(userEmployee.Image);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Could not delete Photo");
                        return View(employeeVM);
                    }
                }
                
                var photoResult = await _photoService.AddPhotoAsync(employeeVM.Image);

                var employee = new Employee
                {
                    Id = id,
                    Image = photoResult.Url.ToString(),
                    Name = employeeVM.Name,
                    Surname = employeeVM.Surname,
                    Login = employeeVM.Login,
                    Password = employeeVM.Password,
                    Salary = employeeVM.Salary,
                    BirthDate = employeeVM.BirthDate,
                    Gender = employeeVM.Gender,
                    WorkingSince = employeeVM.WorkingSince,
                    IsAdmin = employeeVM.IsAdmin
                };

                _employeeRepository.Update(employee);
                return RedirectToAction("Index");
            }
            else
            {
                return View(employeeVM);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var employeeDetails = await _employeeRepository.GetByIdAsync(id);
            if(employeeDetails == null) return View("Error");
            return View(employeeDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employeeDetails = await _employeeRepository.GetByIdAsync(id);
            if (employeeDetails == null) return View("Error");

            _employeeRepository.Delete(employeeDetails);
            return RedirectToAction("Index");
        }
    }
}
