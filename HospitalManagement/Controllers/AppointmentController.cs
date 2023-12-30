using HospitalManagement.Data;
using HospitalManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace HospitalManagement.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly IDoctorService _doctorSer;
        private readonly IAppointmentService _appointSer;
        private readonly ApplicationDbContext _db;

        public AppointmentController(IAppointmentService appointSer, IDoctorService doctorSer, ApplicationDbContext db)
        {
            _doctorSer = doctorSer;
            _appointSer = appointSer;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _doctorSer.GetAllAsync());
        }

        public async Task<IActionResult> Myappointments()
        {
            List<SelectListItem> getDoctors = (
                from e in _db.Doctors.ToList()
                select new SelectListItem {
                    Text = e.FName,
                    Value = e.Id.ToString()
                }
            ).ToList();

            ViewData["Doctors"] = getDoctors;

            return View(await _appointSer.GetAllAsync());
        }

        public IActionResult Create(int id)
        {
            List<SelectListItem> getDoctors = (
                from e in _db.Doctors.ToList()
                select new SelectListItem {
                    Text = e.FName,
                    Value = e.Id.ToString()
                }
            ).ToList();

            ViewData["Doctors"] = getDoctors;
            ViewData["patientCount"] = _db.Appointments.Where(x => x.DoctorId == id).ToList().Count();
            ViewData["DoctorInfo"] = _db.Doctors.FirstOrDefault(x => x.Id == id).FName;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(int id, [Bind("PatientFName, PatientLName, Age, Phone, Disease, Complaint, DateAndTime, DoctorId")] Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                List<SelectListItem> getDoctors = (
                    from e in _db.Doctors.ToList()
                    select new SelectListItem {
                        Text = e.FName,
                        Value = e.Id.ToString()
                    }
                ).ToList();

                ViewData["Doctors"] = getDoctors;

                return View(appointment);
            }

            appointment.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await _appointSer.AddAsync(appointment);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            List<SelectListItem> getDoctors = (
                from e in _db.Doctors.ToList()
                select new SelectListItem {
                    Text = e.FName,
                    Value = e.Id.ToString()
                }
            ).ToList();

            ViewData["Doctors"] = getDoctors;
            
            if (await _appointSer.GetByIdAsync(id) == null)
                return View("NotFound");

            return View(await _appointSer.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id, PatientFName, PatientLName, Age, Phone, Disease, Complaint, DoctorId")] Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                List<SelectListItem> getDoctors = (
                    from e in _db.Doctors.ToList()
                    select new SelectListItem {
                        Text = e.FName,
                        Value = e.Id.ToString()
                    }
                ).ToList();

                ViewData["Doctors"] = getDoctors;

                return View(appointment);
            }

            await _appointSer.UpdateAsync(id, appointment);

            return RedirectToAction("Myappointments", "Appointment");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _appointSer.DeleteAsync(id);

            return RedirectToAction("Index", "Appointment");
        }
    }
}
