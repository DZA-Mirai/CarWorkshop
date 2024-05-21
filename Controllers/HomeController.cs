using CarWorkshop.Data;
using CarWorkshop.Helpers;
using CarWorkshop.Models;
using CarWorkshop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CarWorkshop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDBContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            int? currentId = HttpContext.Session.GetInt32("ID");
            if (currentId != null) return RedirectToAction("Detail", "Employee", new {id = currentId});
            return View();
        }

        [HttpPost]
        public IActionResult Index(LoginViewModel user)
        {
            if(ModelState.IsValid)
            {
                AuthorizationManager authorize = new AuthorizationManager(_context);
                Employee employeeUser = authorize.ValidateEmployee(user.Login, user.Password);
                if (employeeUser == null)
                {
                    ModelState.AddModelError("", "Invalid username or password");
                    return View(user);
                }
                HttpContext.Session.SetInt32("ID", employeeUser.Id);
                return RedirectToAction("Index", "Ticket");
            }
            return RedirectToAction("Index");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("ID");
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
