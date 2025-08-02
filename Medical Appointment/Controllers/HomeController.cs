using System.Diagnostics;

using Medical_Appointment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Medical_Appointment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _context = new();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Doctors(DoctorFilter filters, int page = 1)

        {
            var doctors = _context.Doctors.AsQueryable();

            //filter part

            if (filters.DoctorName is not null)
            {

                doctors = doctors.Where(e => e.Name==filters.DoctorName);
                ViewBag.DoctorName = filters.DoctorName;
            }
            if (filters.Specialization is not null)
            {

                doctors = doctors.Where(e => e.Specialization == filters.Specialization);
                ViewBag.Specialization = filters.Specialization;
            }

            //pass spicialization to droplist 
            var spcializationSelection = _context.Doctors
                .Select(e => e.Specialization)
                .Distinct()
                .ToList();
            ViewBag.droplist=spcializationSelection;


            //pagination part
            int currentpage = page;
            ViewBag.currentpage = currentpage;
            double totalpages = Math.Ceiling(doctors.Count() / 3.0);
            ViewBag.totalPages=totalpages;
            doctors = doctors.Skip((currentpage - 1) * 3).Take(3);

            return View(doctors.ToList());

        }


        public IActionResult AddApointment(int? DoctorId,string? DoctorName="")
        {
            if (DoctorName is not null)
                ViewBag.Doctorname = DoctorName;
            if (DoctorId is not null)
                ViewBag.DoctorId = DoctorId;
            
            return View();
        }


        [HttpPost]
        public IActionResult SaveAppointment(Appointment model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Appointments.Add(model);
                    _context.SaveChanges();
                    TempData["Success"] = "Appointment booked successfully!";
                    //return RedirectToAction("ViewApointment");
                }
                catch (DbUpdateException)
                {
                    TempData["Error"] = "This appointment slot is already booked!";
                }
            }
            else
            {
                TempData["Error"] = "An error occurred while submitting the data.";
            }

            return RedirectToAction("AddApointment", new { DoctorName = model.Doctor?.Name });
        }





        public IActionResult ViewApointment()
        {
            var appointments = _context.Appointments.Include(e=>e.Doctor).OrderBy(e=>e.Date).ThenBy(e=>e.Time);

            return View(appointments.ToList());
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
