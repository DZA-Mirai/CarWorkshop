using CarWorkshop.Data;
using CarWorkshop.Helpers;
using CarWorkshop.Interfaces;
using CarWorkshop.Models;
using CarWorkshop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarWorkshop.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPhotoService _photoService;
        private AuthorizationManager _currentUser;
        private readonly ApplicationDBContext _context;

        public TicketController(ITicketRepository ticketRepository, IEmployeeRepository employeeRepository, IPhotoService photoService, AuthorizationManager currentUser, ApplicationDBContext context)
        {
            _ticketRepository = ticketRepository;
            _employeeRepository = employeeRepository;
            _photoService = photoService;
            _currentUser = currentUser;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            int? currentId = HttpContext.Session.GetInt32("ID");
            if (currentId == null) return RedirectToAction("Index", "Home");
            IEnumerable<Ticket> tickets = await _ticketRepository.GetAll();
            return View(tickets);
        }
        public async Task<IActionResult> Detail(int id)
        {
            Ticket ticket = await _ticketRepository.GetByIdAsync(id);
            var model = new TicketVIewModel
            {
                Ticket = ticket
            };
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            int? currentId = HttpContext.Session.GetInt32("ID");
            if (currentId == null) return RedirectToAction("Index", "Home");
            if (!_currentUser.GetEmployeeRole(currentId)) return RedirectToAction("AccessDenied", "Home");
            var employees = await _employeeRepository.GetAll();
            var employeeList = employees.Select(e => new
            {
                Id = e.Id,
                FullName = e.Name + " " + e.Surname
            });
            ViewBag.Employees = new SelectList(employeeList, "Id", "FullName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTicketViewModel ticketVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(ticketVM.Image);
                var ticket = new Ticket
                {
                    Car = new Car
                    {
                        Brand = ticketVM.Car.Brand,
                        Model = ticketVM.Car.Model,
                        RegId = ticketVM.Car.RegId,
                        Image = result.Url.ToString()
                    },
                    EmployeeId = ticketVM.EmployeeId,
                    Description = ticketVM.Description,
                    DateTimeSlots = new List<DateTimeSlot>
                    {
                        new DateTimeSlot
                        {
                            From = ticketVM.DateTimeSlots.From,
                            Till = ticketVM.DateTimeSlots.Till
                        }
                    }
                };
                _ticketRepository.Add(ticket);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo Upload Failed");
            }

            return View(ticketVM);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            int? currentId = HttpContext.Session.GetInt32("ID");
            if (currentId == null) return RedirectToAction("Index", "Home");
            if (!_currentUser.GetEmployeeRole(currentId)) return RedirectToAction("AccessDenied", "Home");
            var employees = await _employeeRepository.GetAll();
            var employeeList = employees.Select(e => new
            {
                Id = e.Id,
                FullName = e.Name + " " + e.Surname
            });
            ViewBag.Employees = new SelectList(employeeList, "Id", "FullName");
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null) return View("Error");
            var ticketVM = new EditTicketViewModel
            {
                CarId = ticket.CarId,
                Car = ticket.Car,
                URL = ticket.Car.Image,
                EmployeeId = ticket.EmployeeId,
                Description = ticket.Description
            };
            return View(ticketVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditTicketViewModel ticketVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit");
                return View("Edit", ticketVM);
            }

            var userTicket = await _ticketRepository.GetByIdAsyncNoTracking(id);

            if (userTicket != null)
            {
                if(userTicket.Car.Image != null)
                {
                    try
                    {
                        await _photoService.DeletePhotoAsync(userTicket.Car.Image);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Could not delete Photo");
                        return View(ticketVM);
                    }
                }
                var photoResult = await _photoService.AddPhotoAsync(ticketVM.Image);
                ticketVM.Car.Image = photoResult.Url.ToString();

                var ticket = new Ticket
                {
                    Id = id,
                    CarId = ticketVM.CarId,
                    Car = ticketVM.Car,
                    EmployeeId = ticketVM.EmployeeId,
                    Description = ticketVM.Description
                };

                _ticketRepository.Update(ticket);
                return RedirectToAction("Index");
            }
            else
            {
                return View(ticketVM);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            int? currentId = HttpContext.Session.GetInt32("ID");
            if (_currentUser.GetEmployeeRole(currentId))
            {
                var employees = await _employeeRepository.GetAll();
                var employeeList = employees.Select(e => new
                {
                    Id = e.Id,
                    FullName = e.Name + " " + e.Surname
                });
                ViewBag.Employees = new SelectList(employeeList, "Id", "FullName");
                var ticketDetails = await _ticketRepository.GetByIdAsync(id);
                if (ticketDetails == null) return View("Error");
                return View(ticketDetails);
            }
            return RedirectToAction("AccessDenied", "Home");
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticketDetails = await _ticketRepository.GetByIdAsync(id);
            if (ticketDetails == null) return View("Error");

            _ticketRepository.Delete(ticketDetails);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddDate(TicketVIewModel model)
        {
            if(ModelState.IsValid)
            {
                var ticket = await _ticketRepository.GetByIdAsync(model.Ticket.Id);
                if (ticket == null) return View("Error");

                var existingSlots = ticket.DateTimeSlots.ToList();
                foreach(var existingSlot in existingSlots)
                {
                    if(!model.Ticket.DateTimeSlots.Any(s => s.Id == existingSlot.Id))
                    {
                        _ticketRepository.DeleteDates(existingSlot);
                    }
                }

                if(model.Ticket.DateTimeSlots == null)
                {
                    model.Ticket.DateTimeSlots = new List<DateTimeSlot>();
                }

                model.Ticket.DateTimeSlots.Add(model.NewDateTimeSlot);
                _context.Entry(ticket).State = EntityState.Detached;
                _ticketRepository.Update(model.Ticket);
                model.NewDateTimeSlot = new DateTimeSlot();
            }
            return RedirectToAction("Detail", new { id = model.Ticket.Id });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveDate(int ticketId, int slotId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);
            if (ticket == null) return View("Error");
            var slotToRemove = await _ticketRepository.GetSlotByIdAsync(slotId);
            if (slotToRemove != null)
            {
                _ticketRepository.DeleteDates(slotToRemove);
            }
            return RedirectToAction("Detail", new { id = ticketId });
        }

        public IActionResult AddParts(int id)
        {
            var partVM = new PartsViewModel()
            {
                TicketId = id
            };
            return View(partVM);
        }

        [HttpPost]
        public async Task<IActionResult> AddParts(PartsViewModel part)
        {
            if (ModelState.IsValid)
            {
                var ticket = await _ticketRepository.GetByIdAsync(part.TicketId);
                if (ticket == null) return View();
                if(ticket.AdditionalTicketInfo == null)
                {
                    ticket.AdditionalTicketInfo = new AdditionalTicketInfo();
                }
                if(ticket.AdditionalTicketInfo.Parts == null)
                {
                    ticket.AdditionalTicketInfo.Parts = new List<Part>();
                }
                ticket.AdditionalTicketInfo.Parts.Add(part.NewPart);
                _ticketRepository.Update(ticket);
                part.NewPart = new Part();
                return RedirectToAction("Detail", new {id = part.TicketId});
            }
            return View(part);
        }

        [HttpPost]
        public async Task<IActionResult> AddInfo(PartsViewModel part)
        {
            if (ModelState.IsValid)
            {
                var ticket = await _ticketRepository.GetByIdAsync(part.TicketId);
                if (ticket == null) return View();
                if (ticket.AdditionalTicketInfo == null)
                {
                    ticket.AdditionalTicketInfo = new AdditionalTicketInfo();
                }
                if (ticket.AdditionalTicketInfo.Parts == null)
                {
                    ticket.AdditionalTicketInfo.Parts = new List<Part>();
                }
                ticket.AdditionalTicketInfo.Parts.Add(part.NewPart);
                _ticketRepository.Update(ticket);
                part.NewPart = new Part();
                return RedirectToAction("Detail", new { id = part.TicketId });
            }
            return View(part);
        }

        [HttpPost]
        public async Task<IActionResult> RemovePart(int ticketId, int slotId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);
            if (ticket == null) return View("Error");
            var partToRemove = await _ticketRepository.GetPartByIdAsync(slotId);
            if (partToRemove != null)
            {
                _ticketRepository.DeletePart(partToRemove);
            }
            return RedirectToAction("Detail", new { id = ticketId });
        }

        [HttpPost]
        public async Task<IActionResult> AcceptTicket(int ticketId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);
            if (ticket == null) return View("Error");
            ticket.EmployeeId = HttpContext.Session.GetInt32("ID");
            _ticketRepository.Update(ticket);
            return RedirectToAction("Detail", new { id = ticketId });
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsDone(int ticketId, int isReady)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);
            if (ticket == null) return View("Error");
            if(isReady == 0) ticket.IsDone = false;
            else ticket.IsDone = true;
            _ticketRepository.Update(ticket);
            return RedirectToAction("Detail", new {id = ticketId});
        }

        [HttpPost]
        public async Task<IActionResult> Archive(int ticketId, int isClosed)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);
            if (ticket == null) return View("Error");
            if(isClosed == 0) ticket.IsClosed = false;
            else
            {
                ticket.IsClosed = true;
                ticket.IsDone = false;
            }
            _ticketRepository.Update(ticket);
            return RedirectToAction("Index");
        }
    }
}
